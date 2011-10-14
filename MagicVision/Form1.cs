/* Magic Vision
 * Created by Peter Simard
 * You are free to use this source code any way you wish, all I ask for is an attribution
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using DirectX.Capture;
using Microsoft.VisualBasic;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace PoolVision
{
    public partial class Form1 : Form
    {
        private Bitmap cameraBitmap;
        private Bitmap cameraBitmapLive;
        private Bitmap filteredBitmap;
        private Bitmap cardBitmap;
        private Bitmap cardArtBitmap;
        private String refCardDir = @"C:\Users\Pete\Pictures\New Phyrexia\Crops\";
        private Capture capture = null;
        private Filters cameraFilters = new Filters();
        private List<MagicCard> magicCards = new List<MagicCard>();
        private List<MagicCard> magicCardsLastFrame = new List<MagicCard>();
        private List<ReferenceCard> referenceCards = new List<ReferenceCard>();
        static readonly object _locker = new object();

        public static string SqlConString = "SERVER=127.0.0.1;" +
                "DATABASE=magiccards;" +
                "UID=root;" +
                "Allow Zero Datetime=true";

        public MySqlClient sql = new MySqlClient(SqlConString);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ReferenceCard card in referenceCards)
            {
                Phash.ph_dct_imagehash(refCardDir + (String)card.dataRow["Set"] + "\\" + card.cardId + ".jpg", ref card.pHash);
                sql.dbNone("UPDATE cards SET pHash=" + card.pHash.ToString() + " WHERE id=" + card.cardId);
            }
        }

        double GetDeterminant(double x1, double y1, double x2, double y2)
        {
            return x1 * y2 - x2 * y1;
        }

        double GetArea(IList<IntPoint> vertices)
        {
            if (vertices.Count < 3)
            {
                return 0;
            }
            double area = GetDeterminant(vertices[vertices.Count - 1].X, vertices[vertices.Count - 1].Y, vertices[0].X, vertices[0].Y);
            for (int i = 1; i < vertices.Count; i++)
            {
                area += GetDeterminant(vertices[i - 1].X, vertices[i - 1].Y, vertices[i].X, vertices[i].Y);
            }
            return area / 2;
        }

        private void detectQuads(Bitmap bitmap)
        {
            // Greyscale
            filteredBitmap = Grayscale.CommonAlgorithms.BT709.Apply(bitmap);

            // edge filter
            SobelEdgeDetector edgeFilter = new SobelEdgeDetector();
            edgeFilter.ApplyInPlace(filteredBitmap);

            // Threshhold filter
            Threshold threshholdFilter = new Threshold(190);
            threshholdFilter.ApplyInPlace(filteredBitmap);

            BitmapData bitmapData = filteredBitmap.LockBits(
                new Rectangle(0, 0, filteredBitmap.Width, filteredBitmap.Height),
                ImageLockMode.ReadWrite, filteredBitmap.PixelFormat);

 
            BlobCounter blobCounter = new BlobCounter();

            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 125;
            blobCounter.MinWidth = 125;

            blobCounter.ProcessImage(bitmapData);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            filteredBitmap.UnlockBits(bitmapData);

            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

            Bitmap bm = new Bitmap(filteredBitmap.Width, filteredBitmap.Height, PixelFormat.Format24bppRgb);

            Graphics g = Graphics.FromImage(bm);
            g.DrawImage(filteredBitmap, 0, 0);

            Pen pen = new Pen(Color.Red, 5);
            List<IntPoint> cardPositions = new List<IntPoint>();


            // Loop through detected shapes
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners;
                bool sameCard = false;

                // is triangle or quadrilateral
                if (shapeChecker.IsConvexPolygon(edgePoints, out corners))
                {
                    // get sub-type
                    PolygonSubType subType = shapeChecker.CheckPolygonSubType(corners);

                    // Only return 4 corner rectanges
                    if ((subType == PolygonSubType.Parallelogram || subType == PolygonSubType.Rectangle) &&  corners.Count == 4)
                    {
                        // Check if its sideways, if so rearrange the corners so it's veritcal
                        rearrangeCorners(corners);

                        // Prevent it from detecting the same card twice
                        foreach (IntPoint point in cardPositions)
                        {
                            if (corners[0].DistanceTo(point) < 40)
                                sameCard = true;
                        }
                        
                        if (sameCard)
                            continue;

                        // Hack to prevent it from detecting smaller sections of the card instead of the whole card
                        if (GetArea(corners) < 20000)
                            continue;
                         
                        cardPositions.Add(corners[0]);

                        g.DrawPolygon(pen, ToPointsArray(corners));

                        // Extract the card bitmap
                        QuadrilateralTransformation transformFilter = new QuadrilateralTransformation(corners, 211, 298);
                        cardBitmap = transformFilter.Apply(cameraBitmap);

                        List<IntPoint> artCorners = new List<IntPoint>();
                        artCorners.Add(new IntPoint(14, 35));
                        artCorners.Add(new IntPoint(193, 35));
                        artCorners.Add(new IntPoint(193, 168));
                        artCorners.Add(new IntPoint(14, 168));

                        // Extract the art bitmap
                        QuadrilateralTransformation cartArtFilter = new QuadrilateralTransformation(artCorners, 183, 133);
                        cardArtBitmap = cartArtFilter.Apply(cardBitmap);

                        MagicCard card = new MagicCard();
                        card.corners = corners;
                        card.cardBitmap = cardBitmap;
                        card.cardArtBitmap = cardArtBitmap;

                        magicCards.Add(card);
                    }
                } 
            }

            pen.Dispose();
            g.Dispose();

            filteredBitmap = bm;
        }

        // Move the corners a fixed amount
        private void shiftCorners(List<IntPoint> corners, IntPoint point)
        {
            int xOffset = point.X - corners[0].X;
            int yOffset = point.Y - corners[0].Y;

            for (int x = 0; x < corners.Count; x++)
            {
                IntPoint point2 = corners[x];

                point2.X += xOffset;
                point2.Y += yOffset;

                corners[x] = point2;
            }
        }


        private void rearrangeCorners(List<IntPoint> corners)
        {
            float[] pointDistances = new float[4];

            for (int x = 0; x < corners.Count; x++)
            {
                IntPoint point = corners[x];

                pointDistances[x] = point.DistanceTo( (x == (corners.Count - 1) ? corners[0] : corners[x + 1]) ) ;
            }

            float shortestDist = float.MaxValue;
            Int32 shortestSide = Int32.MaxValue;

            for (int x = 0; x < corners.Count; x++)
            {
                if (pointDistances[x] < shortestDist)
                {
                    shortestSide = x;
                    shortestDist = pointDistances[x];
                }
            }

            if (shortestSide != 0 && shortestSide != 2)
            {
                IntPoint endPoint = corners[0];
                corners.RemoveAt(0);
                corners.Add(endPoint);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            cameraBitmap = new Bitmap(640, 480);
            capture = new Capture(cameraFilters.VideoInputDevices[0], cameraFilters.AudioInputDevices[0]);
            VideoCapabilities vc = capture.VideoCaps;
            capture.FrameSize = new Size(640, 480);
            capture.PreviewWindow = cam;
			capture.FrameEvent2 += new Capture.HeFrame(CaptureDone);
			capture.GrapImg();

            loadSourceCards();
        }

        private void loadSourceCards()
        {
            using (DataTable Reader = sql.dbResult("SELECT * FROM cards"))
            {
                foreach (DataRow r in Reader.Rows)
                {
                    ReferenceCard card = new ReferenceCard();
                    card.cardId = (String)r["id"];
                    card.name = (String)r["Name"];
                    card.pHash = (UInt64)r["pHash"];
                    card.dataRow = r;

                    referenceCards.Add(card);
                }
            }
        }

        private void CaptureDone(System.Drawing.Bitmap e)
        {
            lock (_locker)
            {
                magicCardsLastFrame = new List<MagicCard>(magicCards);
                magicCards.Clear();
                cameraBitmap = e;
                cameraBitmapLive = (Bitmap)cameraBitmap.Clone();
                detectQuads(cameraBitmap);
                matchCard();

                image_output.Image = filteredBitmap;
                camWindow.Image = cameraBitmap;                
            }
        }

        private void matchCard()
        {
            int cardTempId = 0;
            foreach(MagicCard card in magicCards)
            {
                cardTempId++;
                // Write the image to disk to be read by the pHash library.. should really find
                // a way to pass a pointer to image data directly
                card.cardArtBitmap.Save("tempCard" + cardTempId + ".jpg", ImageFormat.Jpeg);


                // Calculate art bitmap hash
                UInt64 cardHash = 0;
                Phash.ph_dct_imagehash("tempCard" + cardTempId + ".jpg", ref cardHash);

                int lowestHamming = int.MaxValue;
                ReferenceCard bestMatch = null;

                foreach (ReferenceCard referenceCard in referenceCards)
                {
                    int hamming = Phash.HammingDistance(referenceCard.pHash, cardHash);
                    if (hamming < lowestHamming)
                    {
                        lowestHamming = hamming;
                        bestMatch = referenceCard;
                    }
                }

                if (bestMatch != null)
                {
                    card.referenceCard = bestMatch;
                    //Debug.WriteLine("Highest Similarity: " + bestMatch.name + " ID: " + bestMatch.cardId.ToString());
                    
                    Graphics g = Graphics.FromImage(cameraBitmap);
                    g.DrawString(bestMatch.name, new Font("Tahoma", 25), Brushes.Black, new PointF(card.corners[0].X - 29, card.corners[0].Y - 39));
                    g.DrawString(bestMatch.name, new Font("Tahoma", 25), Brushes.Yellow, new PointF(card.corners[0].X - 30, card.corners[0].Y - 40));
                    g.Dispose();
                }
            }
        }
         

        // Conver list of AForge.NET's points to array of .NET points
        private System.Drawing.Point[] ToPointsArray(List<IntPoint> points)
        {
            System.Drawing.Point[] array = new System.Drawing.Point[points.Count];

            for (int i = 0, n = points.Count; i < n; i++)
            {
                array[i] = new System.Drawing.Point(points[i].X, points[i].Y);
            }

            return array;
        }

        private void camWindow_MouseClick(object sender, MouseEventArgs e)
        {
            lock (_locker)
            {
                foreach (MagicCard card in magicCards)
                {
                    Rectangle rect = new Rectangle(card.corners[0].X, card.corners[0].Y, (card.corners[1].X - card.corners[0].X), (card.corners[2].Y - card.corners[1].Y));
                    if (rect.Contains(e.Location))
                    {
                        Debug.WriteLine(card.referenceCard.name);
                        cardArtImage.Image = card.cardArtBitmap;
                        cardImage.Image = card.cardBitmap;

                        cardInfo.Text = "Card Name: " + card.referenceCard.name + Environment.NewLine + 
                            "Set: " + (String)card.referenceCard.dataRow["Set"] + Environment.NewLine + 
                            "Type: " + (String)card.referenceCard.dataRow["Type"] + Environment.NewLine + 
                            "Casting Cost: " + (String)card.referenceCard.dataRow["Cost"] + Environment.NewLine + 
                            "Rarity: " + (String)card.referenceCard.dataRow["Rarity"] + Environment.NewLine;

                    }
                }
            }
        }
    }

    class ReferenceCard
    {
        public string cardId;
        public string name;
        public UInt64 pHash;
        public DataRow dataRow;
    }

    class MagicCard
    {
        public ReferenceCard referenceCard;
        public List<IntPoint> corners;
        public Bitmap cardBitmap;
        public Bitmap cardArtBitmap;
    }

    public class Phash
    {

        [DllImport("pHash.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ph_dct_imagehash(string file_name, ref UInt64 Hash);

        private static UInt64 m1 = 0x5555555555555555;
        private static UInt64 m2 = 0x3333333333333333;
        private static UInt64 h01 = 0x0101010101010101;
        private static UInt64 m4 = 0x0f0f0f0f0f0f0f0f;

        // Calculate the similarity between two hashes
        public static int HammingDistance(UInt64 hash1, UInt64 hash2)
        {
            UInt64 x = hash1 ^ hash2;


            x -= (x >> 1) & m1;
            x = (x & m2) + ((x >> 2) & m2);
            x = (x + (x >> 4)) & m4;
            UInt64 res = (x * h01) >> 56;

            return (int)res;
        }
    }

}
