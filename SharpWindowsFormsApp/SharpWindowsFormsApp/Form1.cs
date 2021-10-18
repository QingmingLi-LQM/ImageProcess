using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MvCamCtrl.NET;
using OpenCvSharp;

namespace SharpWindowsFormsApp
{
    public partial class Form1 : Form
    {
        private string path = @"D:\3D\Images\7-15\1-1\1-1-1.bmp";
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Mat src = new Mat(path, ImreadModes.Grayscale);
            // Mat src = Cv2.ImRead("lenna.png", ImreadModes.GrayScale);
            Mat dst = new Mat();

            Cv2.Canny(src, dst, 50, 200);
            using (new Window("src image", src))
            using (new Window("dst image", dst))
            {
                string StatusStr = "11011001";
                if(StatusStr.Length == 8)
                {
                    string key_word = "1";
                    int index = 0;
                    int count = 0;
                    int[] indexPos = new int[0];
                    while((index = StatusStr.IndexOf(key_word, index)) != -1)
                    {
                        count++;
                        Array.Resize(ref indexPos, count);
                        indexPos[count - 1] = index;
                        index = index + key_word.Length;
                    }
                    Console.WriteLine("1出现的次数:{0}", count);
                    for(int i = 0; i < indexPos.Length; i++)
                    {
                        switch (indexPos[i])
                        {
                            case 0:
                                Console.WriteLine("电源打开！");
                                break;
                            case 1:
                                Console.WriteLine("HDMI电源打开！");
                                break;
                            case 2:
                                Console.WriteLine("默认位置为0");
                                break;
                            case 3:
                                Console.WriteLine("HDMI输入正常！");
                                break;
                            case 4:
                                Console.WriteLine("风扇2运行！");
                                break;
                            case 5:
                                Console.WriteLine("风扇1运行！");
                                break;
                            case 6:
                                Console.WriteLine("LED打开！");
                                break;
                            case 7:
                                Console.WriteLine("DLP打开！");
                                break;
                        }
                    }
                }
                Cv2.WaitKey();
            }
        }

        private void singleBtn_Click(object sender, EventArgs e)
        {
            ReadImageSingleChannelPixel(path);
        }

        private void multiBtn_Click(object sender, EventArgs e)
        {
            ReadImageThreeChannelPixel(path);
        }

        //单通道像素值
        private void ReadImageSingleChannelPixel(string imagePath)
        {
            using (Mat src = new Mat(path, ImreadModes.AnyColor | ImreadModes.AnyDepth))
            using(Mat dst = new Mat())
            {
                //ColorConversionCodes 色彩空间转换枚举类型 BGRA2GRAY 转为灰度单通道
                Cv2.CvtColor(src, dst, ColorConversionCodes.BGRA2GRAY);
                Mat gray = new Mat();
                dst.CopyTo(gray);

                //获取图片的行(高度)
                int height = dst.Rows;
                //获取图片的列（宽度）
                int width = dst.Cols;

                for (int row = 0; row < height; row++)
                {
                    for (int col = 0; col < width; col++)
                    {
                        //获取对应矩阵坐标的像素
                        byte p = dst.At<byte>(row, col);
                        //反转像素值
                        byte value = byte.Parse((255 - p).ToString());
                        //赋值
                        dst.Set(row, col, value);
                    }
                }
                //单通道图像
                using (new Window("gray", gray))
                //反转后的单通道图像
                using (new Window("dst", dst))
                using (new Window("src", src))
                {
                    Cv2.WaitKey();
                }
            }
            
        }

        //三通道像素值
        private void ReadImageThreeChannelPixel(string imagePath)
        {
            using (Mat src = new Mat(imagePath, ImreadModes.AnyColor | ImreadModes.AnyDepth))
            using (Mat dst = new Mat(src.Size(), src.Type()))
            {
                int height = src.Rows;
                int width = src.Cols;
                int cn = src.Channels();

                //反转像素法
                for (int row = 0; row < height; row++)
                {
                    for(int col = 0; col < width; col++)
                    {
                        //如果是单通道
                        if(cn == 1)
                        {
                            //获取像素
                            byte p = dst.At<byte>(row, col);
                            //反转像素
                            byte value = byte.Parse((255 - p).ToString());
                            //赋值
                            dst.Set(row, col, value);
                        }else if(cn == 3)
                        {
                            //读取原图的像素
                            int b = src.At<Vec3b>(row, col)[0];
                            int g = src.At<Vec3b>(row, col)[1];
                            int r = src.At<Vec3b>(row, col)[2];
                            //反转像素
                            Vec3b color = new Vec3b
                            {
                                Item0 = (byte)Math.Abs(src.Get<Vec3b>(row, col).Item0 - 255),
                                Item1 = (byte)Math.Abs(src.Get<Vec3b>(row, col).Item1 - 255),
                                Item2 = (byte)Math.Abs(src.Get<Vec3b>(row, col).Item2 - 255)
                            };
                            //赋值
                            dst.Set(row, col, color);
                        }
                    }
                }
                //反转
                using(new Window("dst", dst))
                //原图
                using(new Window("src", src))
                {
                    Cv2.WaitKey(0);
                }
            }
        }

        private void stitchImageBtn_Click(object sender, EventArgs e)
        {
            string imgPath1 = @"C:\Data\3D\Images\1-1-1.bmp";
            string imgPath2 = @"C:\Data\3D\Images\1-1-2.bmp";
            
            using (Mat src1 = new Mat(imgPath1, ImreadModes.AnyColor | ImreadModes.AnyDepth))
            using (Mat src2 = new Mat(imgPath2, ImreadModes.AnyColor | ImreadModes.AnyDepth))
            using(Mat dst = new Mat())
            {
                //Cv2.CvtColor(src1, dst, ColorConversionCodes.BGR2BGRA);
                double alpha = 0.5;
                Console.WriteLine("Type=" + src2.Size());
                if(src1.Rows == src2.Rows && src1.Cols == src2.Cols && src1.Type() == src2.Type())
                {
                    Cv2.AddWeighted(src1, alpha, src2, (1 - alpha), 0, dst);
                    using(new Window("dst", dst))
                    using(new Window("src1", src1))
                    using(new Window("src2", src2))
                    {
                        Cv2.WaitKey(0);
                    }
                }
                else
                {
                    Console.WriteLine("图片类型不一致");
                }
            }

            Mat src_1 = Cv2.ImRead(imgPath1);
            Mat dst1 = Cv2.ImRead(imgPath2);

            Mat src_mask = 255 * Mat.Ones(src_1.Size(), MatType.CV_8UC3);
            OpenCvSharp.Point center = new OpenCvSharp.Point(dst1.Cols / 2, dst1.Rows / 2);

            Mat normal_clone = new Mat();
            Mat mixed_clone = new Mat();
            Cv2.SeamlessClone(src_1, dst1, src_mask, center, normal_clone, SeamlessCloneMethods.NormalClone);
            Cv2.SeamlessClone(src_1, dst1, src_mask, center, mixed_clone, SeamlessCloneMethods.MixedClone);

            Cv2.ImShow("ROI_img", src_1);
            Cv2.ImShow("bg_img", dst1);
            Cv2.ImShow("normal_clone", normal_clone);
            Cv2.ImShow("mixed_clone", mixed_clone);
            Cv2.WaitKey(0);
        }

        private void imageSpiliteBtn_Click(object sender, EventArgs e)
        {
            TestImageSpilite();
        }

        private void TestImageSpilite()
        {
            string imgPath1 = @"D:\3D\Images\7-15\1-2\1-2-";
            string newImgPath = @"D:\3D\Images\7-15\Output\1-2\1-2-";
            StringBuilder stringBuilder = new StringBuilder();
            int widthN = 5;
            int heightN = 10;
            int kSubstract = 50;
            float[] dataWeight = new float[widthN];
            float[] newWeight = new float[widthN];
            float[] dataWeightHeight = new float[heightN];
            float[] newWeightHeight = new float[heightN];
            Mat newImg = new Mat();
            int bottomIndex = 680;
            
            using (Mat img_src = new Mat(Convert.ToString(imgPath1 + 1 + ".bmp"), ImreadModes.AnyColor | ImreadModes.AnyDepth))
            {
                Mat matNew = new Mat();
                Cv2.Resize(img_src, matNew, new OpenCvSharp.Size(2750, 750));
                Console.WriteLine("newImgType=" + newImg.Type());
                Console.WriteLine("newImgChannels=" + newImg.Channels());
                Cv2.CvtColor(matNew, newImg, ColorConversionCodes.BGR2BGRA);
                for (int i = 0; i < newImg.Cols; i++)
                {
                    for(int j = 0; j < newImg.Rows; j++)
                    {
                        Vec4b coloaNew = new Vec4b
                        {
                            Item0 = Convert.ToByte(newImg.Get<Vec4b>(j, i).Item0 * 0),
                            Item1 = Convert.ToByte(newImg.Get<Vec4b>(j, i).Item1 * 0),
                            Item2 = Convert.ToByte(newImg.Get<Vec4b>(j, i).Item2 * 0),
                        };
                        newImg.Set(j, i, coloaNew);
                    }
                }
            }
            for (int k = 1; k <= 60; k++)
            {
                int m = 0;
                stringBuilder.Append(imgPath1 + k + ".bmp");
                Console.WriteLine(stringBuilder);
                using(Mat img = new Mat(Convert.ToString(stringBuilder), ImreadModes.AnyColor | ImreadModes.AnyDepth))
                using(Mat dst = new Mat())
                {
                    Mat result = new Mat();
                    Mat img_1 = new Mat();
                    /*int sizeW = 0, sizeSubstratc = 0;
                    if (k == 1)
                    {
                        sizeW = 80;
                    }
                    else
                    {
                        sizeW = 80;
                    }
                    OpenCvSharp.Size mSize = new OpenCvSharp.Size(sizeW, 77);
                    Cv2.Resize(img, dst, mSize);*/
                    /*Mat rotatal = Cv2.GetRotationMatrix2D(new Point2f(img.Width / 2, img.Height / 2), -90, 1);
                    Cv2.WarpAffine(img, result, rotatal, img.Size());
                    Cv2.CvtColor(result, img_1, ColorConversionCodes.BGR2BGRA);*/

                    //Mat newResult = new Mat(img_1.Rows, img_1.Cols - 20, img_1.Type());
                    Mat newResult = new Mat();
                    img.CopyTo(newResult);
                    Console.WriteLine("Channels=" + newResult.Channels());
                    /*int h = 0;
                    for(int i = 2; i < img_1.Cols - 18; i++)
                    {
                        for(int j = 0; j < img_1.Rows; j++)
                        {
                            if(img_1.Channels() == 1)
                            {

                            }else if(img_1.Channels() == 4)
                            {
                                Vec4b colorNew = new Vec4b
                                {
                                    Item0 = Convert.ToByte(img_1.Get<Vec4b>(j, i).Item0),
                                    Item1 = Convert.ToByte(img_1.Get<Vec4b>(j, i).Item1),
                                    Item2 = Convert.ToByte(img_1.Get<Vec4b>(j, i).Item2),
                                    Item3 = Convert.ToByte(img_1.Get<Vec4b>(j, i).Item3)
                                };
                                newResult.Set(j, h, colorNew);
                            }
                        }
                        h++;
                    }*/

                    for (int i = newResult.Cols - widthN; i < newResult.Cols; i++)
                    {
                        dataWeight[m] = i;
                        Console.WriteLine("i=" + dataWeight[m]);
                        m++;
                    }
                    for (int i = 0; i < dataWeight.Length; i++)
                    {
                        Console.WriteLine("dataWeight=" + dataWeight[i]);
                    }
                    float dataMax = dataWeight.Max();
                    float dataMin = dataWeight.Min();
                    for (int i = 0; i < dataWeight.Length; i++)
                    {
                        newWeight[i] = (dataWeight[i] - dataMin) / (dataMax - dataMin);
                        Console.WriteLine("newWeight=" + newWeight[i]);
                    }
                    //newResult.SaveImage(newImgPath + k + "_1.bmp");
                    m = 0;
                    for (int i = newResult.Cols - widthN; i < newResult.Cols; i++)
                    {
                        for (int j = 0; j < newResult.Rows; j++)
                        {
                            float f = 0;
                            if (newWeight[m] >= 0 && newWeight[m] <= 0.5)
                            {
                                f = Convert.ToSingle(0.5 * Math.Pow(2 * newWeight[m], 2));

                            }
                            else if (newWeight[m] >= 0.5 && newWeight[m] <= 1)
                            {
                                f = Convert.ToSingle(1 - (1 - 0.5) * Math.Pow(2 * (1 - newWeight[m]), 2));
                            }
                            Vec4b colorA = new Vec4b
                            {
                                Item0 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item0 * (1 - f)),
                                Item1 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item1 * (1 - f)),
                                Item2 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item2 * (1 - f)),
                                Item3 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item3 * (1 - f))
                            };
                            newResult.Set(j, i, colorA);
                        }
                        m++;
                    }
                    
                    m = 0;
                    if(k == 1)
                    {
                        for (int i = newResult.Rows - 1; i < newResult.Rows; i++)
                        {
                            for (int j = 0; j < newResult.Cols; j++)
                            {
                                float f = 0;
                                if (newWeight[m] >= 0 && newWeight[m] <= 0.5)
                                {
                                    f = Convert.ToSingle(0.5 * Math.Pow(2 * newWeight[m], 2));

                                }
                                else if (newWeight[m] >= 0.5 && newWeight[m] <= 1)
                                {
                                    f = Convert.ToSingle(1 - (1 - 0.5) * Math.Pow(2 * (1 - newWeight[m]), 2));
                                }
                                Vec4b colorA = new Vec4b
                                {
                                    Item0 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item0 * (1 - f)),
                                    Item1 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item1 * (1 - f)),
                                    Item2 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item2 * (1 - f)),
                                    Item3 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item3 * (1 - f))
                                };
                                newResult.Set(i, j, colorA);
                            }
                            m++;
                        }
                    }
                    m = 0;
                    for (int i = 0; i < widthN; i++)
                    {
                        for (int j = 0; j < newResult.Rows; j++)
                        {
                            float f = 0;
                            if (newWeight[m] >= 0 && newWeight[m] <= 0.5)
                            {
                                f = Convert.ToSingle(0.5 * Math.Pow(2 * newWeight[m], 2));

                            }
                            else if (newWeight[m] >= 0.5 && newWeight[m] <= 1)
                            {
                                f = Convert.ToSingle(1 - (1 - 0.5) * Math.Pow(2 * (1 - newWeight[m]), 2));
                            }
                            Vec4b colorA = new Vec4b
                            {
                                Item0 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item0 * f),
                                Item1 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item1 * f),
                                Item2 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item2 * f),
                                Item3 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item3 * f)
                            };
                            newResult.Set(j, i, colorA);
                        }
                        m++;
                    }

                    m = 0;
                    if (k == 60)
                    {
                        for (int i = newResult.Rows - widthN; i < newResult.Rows; i++)
                        {
                            for (int j = 0; j < newResult.Cols; j++)
                            {
                                float f = 0;
                                if (newWeight[m] >= 0 && newWeight[m] <= 0.5)
                                {
                                    f = Convert.ToSingle(0.5 * Math.Pow(2 * newWeight[m], 2));

                                }
                                else if (newWeight[m] >= 0.5 && newWeight[m] <= 1)
                                {
                                    f = Convert.ToSingle(1 - (1 - 0.5) * Math.Pow(2 * (1 - newWeight[m]), 2));
                                }
                                Vec4b colorA = new Vec4b
                                {
                                    Item0 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item0 * (1 - f)),
                                    Item1 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item1 * (1 - f)),
                                    Item2 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item2 * (1 - f)),
                                    Item3 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item3 * (1 - f))
                                };
                                newResult.Set(i, j, colorA);
                            }
                            m++;
                        }
                    }

                    newResult.SaveImage(newImgPath + k + ".bmp");

                    if (k == 1)
                    {
                        for (int ki = 0; ki < newResult.Cols; ki++)
                        {
                            for (int kj = 0; kj < newResult.Rows; kj++)
                            {
                                Vec4b colorNew = new Vec4b
                                {
                                    Item0 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item0),
                                    Item1 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item1),
                                    Item2 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item2),
                                    Item3 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item3)
                                };
                                newImg.Set(kj, ki, colorNew);
                            }
                        }
                    }
                    else
                    {
                        int km = 0, kt=widthN;
                        for (int ki = 0, kh = (k - 1) * kSubstract - (k - 1) * kt; ki < newResult.Cols; ki++)
                        {
                            for(int kj = 0; kj < newResult.Rows; kj++)
                            {
                                Vec4b colorNew = newResult.Get<Vec4b>(kj, ki);
                                if (km < widthN)
                                {
                                    if(newResult.Get<Vec4b>(kj, ki).Item0 + newImg.Get<Vec4b>(kj, kh).Item0 > 255)
                                    {
                                        colorNew.Item0 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item0 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item0 + newImg.Get<Vec4b>(kj, kh).Item0);
                                    }
                                    if (newResult.Get<Vec4b>(kj, ki).Item1 + newImg.Get<Vec4b>(kj, kh).Item1 > 255)
                                    {
                                        colorNew.Item1 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item1 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item1 + newImg.Get<Vec4b>(kj, kh).Item1);
                                    }
                                    if (newResult.Get<Vec4b>(kj, ki).Item2 + newImg.Get<Vec4b>(kj, kh).Item2 > 255)
                                    {
                                        colorNew.Item2 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item2 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item2 + newImg.Get<Vec4b>(kj, kh).Item2);
                                    }
                                    if (newResult.Get<Vec4b>(kj, ki).Item3 + newImg.Get<Vec4b>(kj, kh).Item3 > 255)
                                    {
                                        colorNew.Item3 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item3 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item3 + newImg.Get<Vec4b>(kj, kh).Item3);
                                    }
                                }
                                else
                                {
                                    colorNew.Item0 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item0);
                                    colorNew.Item1 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item1);
                                    colorNew.Item2 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item2);
                                    colorNew.Item3 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item3);
                                }
                                newImg.Set(kj, kh, colorNew);
                            }
                            km++;
                            kh++;
                        }
                    }
                }
                stringBuilder.Clear();
                
            }
            newImg?.SaveImage(@"D:\3D\Images\7-15\Output\1-2\OutPut.bmp");

            //1-2部分
            string imgPath1_2 = @"D:\3D\Images\7-15\1-1\1-1-";
            string newImgPath1_2 = @"D:\3D\Images\7-15\Output\1-1\1-1-";
            for (int k = 1; k <= 60; k++)
            {
                int m = 0;
                stringBuilder.Append(imgPath1_2 + k + ".bmp");
                Console.WriteLine(stringBuilder);
                using (Mat img = new Mat(Convert.ToString(stringBuilder), ImreadModes.AnyColor | ImreadModes.AnyDepth))
                using (Mat dst = new Mat())
                {
                    Mat result = new Mat();
                    Mat img_1 = new Mat();
                    Mat newResult = new Mat();
                    img.CopyTo(newResult);
                    Console.WriteLine("Channels=" + newResult.Channels());

                    for (int i = newResult.Cols - widthN; i < newResult.Cols; i++)
                    {
                        dataWeight[m] = i;
                        Console.WriteLine("i=" + dataWeight[m]);
                        m++;
                    }
                    for (int i = 0; i < dataWeight.Length; i++)
                    {
                        Console.WriteLine("dataWeight=" + dataWeight[i]);
                    }
                    float dataMax = dataWeight.Max();
                    float dataMin = dataWeight.Min();
                    for (int i = 0; i < dataWeight.Length; i++)
                    {
                        newWeight[i] = (dataWeight[i] - dataMin) / (dataMax - dataMin);
                        Console.WriteLine("newWeight=" + newWeight[i]);
                    }
                    //newResult.SaveImage(newImgPath + k + "_1.bmp");
                    m = 0;
                    for (int i = newResult.Cols - widthN; i < newResult.Cols; i++)
                    {
                        for (int j = 0; j < newResult.Rows; j++)
                        {
                            float f = 0;
                            if (newWeight[m] >= 0 && newWeight[m] <= 0.5)
                            {
                                f = Convert.ToSingle(0.5 * Math.Pow(2 * newWeight[m], 2));

                            }
                            else if (newWeight[m] >= 0.5 && newWeight[m] <= 1)
                            {
                                f = Convert.ToSingle(1 - (1 - 0.5) * Math.Pow(2 * (1 - newWeight[m]), 2));
                            }
                            Vec4b colorA = new Vec4b
                            {
                                Item0 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item0 * (1 - f)),
                                Item1 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item1 * (1 - f)),
                                Item2 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item2 * (1 - f)),
                                Item3 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item3 * (1 - f))
                            };
                            newResult.Set(j, i, colorA);
                        }
                        m++;
                    }
                    m = 0;
                    if (k == 1)
                    {
                        for (int i = 0; i < widthN; i++)
                        {
                            for (int j = 0; j < newResult.Cols; j++)
                            {
                                float f = 0;
                                if (newWeight[m] >= 0 && newWeight[m] <= 0.5)
                                {
                                    f = Convert.ToSingle(0.5 * Math.Pow(2 * newWeight[m], 2));

                                }
                                else if (newWeight[m] >= 0.5 && newWeight[m] <= 1)
                                {
                                    f = Convert.ToSingle(1 - (1 - 0.5) * Math.Pow(2 * (1 - newWeight[m]), 2));
                                }
                                Vec4b colorA = new Vec4b
                                {
                                    Item0 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item0 * f),
                                    Item1 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item1 * f),
                                    Item2 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item2 * f),
                                    Item3 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item3 * f)
                                };
                                newResult.Set(i, j, colorA);
                            }
                            m++;
                        }
                    }
                    m = 0;
                    for (int i = 0; i < widthN; i++)
                    {
                        for (int j = 0; j < newResult.Rows; j++)
                        {
                            float f = 0;
                            if (newWeight[m] >= 0 && newWeight[m] <= 0.5)
                            {
                                f = Convert.ToSingle(0.5 * Math.Pow(2 * newWeight[m], 2));

                            }
                            else if (newWeight[m] >= 0.5 && newWeight[m] <= 1)
                            {
                                f = Convert.ToSingle(1 - (1 - 0.5) * Math.Pow(2 * (1 - newWeight[m]), 2));
                            }
                            Vec4b colorA = new Vec4b
                            {
                                Item0 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item0 * f),
                                Item1 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item1 * f),
                                Item2 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item2 * f),
                                Item3 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item3 * f)
                            };
                            newResult.Set(j, i, colorA);
                        }
                        m++;
                    }

                    m = 0;
                    if (k == 60)
                    {
                        for (int i = 0; i < widthN; i++)
                        {
                            for (int j = 0; j < newResult.Cols; j++)
                            {
                                float f = 0;
                                if (newWeight[m] >= 0 && newWeight[m] <= 0.5)
                                {
                                    f = Convert.ToSingle(0.5 * Math.Pow(2 * newWeight[m], 2));

                                }
                                else if (newWeight[m] >= 0.5 && newWeight[m] <= 1)
                                {
                                    f = Convert.ToSingle(1 - (1 - 0.5) * Math.Pow(2 * (1 - newWeight[m]), 2));
                                }
                                Vec4b colorA = new Vec4b
                                {
                                    Item0 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item0 * f),
                                    Item1 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item1 * f),
                                    Item2 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item2 * f),
                                    Item3 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item3 * f)
                                };
                                newResult.Set(i, j, colorA);
                            }
                            m++;
                        }
                    }

                    
                    newResult.SaveImage(newImgPath1_2 + k + ".bmp");
                    if (k == 1)
                    {
                        for (int ki = 0; ki < newResult.Cols; ki++)
                        {
                            for (int kj = 0; kj < newResult.Rows; kj++)
                            {
                                Vec4b colorNew = new Vec4b
                                {
                                    Item0 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item0),
                                    Item1 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item1),
                                    Item2 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item2),
                                    Item3 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item3)
                                };
                                newImg.Set(bottomIndex + kj, ki, colorNew);
                            }
                        }
                    }
                    else
                    {
                        int km = 0, kt = widthN;
                        for (int ki = 0, kh = (k - 1) * kSubstract - (k - 1) * kt; ki < newResult.Cols; ki++)
                        {
                            for (int kj = 0; kj < newResult.Rows; kj++)
                            {
                                Vec4b colorNew = newResult.Get<Vec4b>(kj, ki);
                                if (km < widthN)
                                {
                                    if (newResult.Get<Vec4b>(kj, ki).Item0 + newImg.Get<Vec4b>(bottomIndex + kj, kh).Item0 > 255)
                                    {
                                        colorNew.Item0 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item0 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item0 + newImg.Get<Vec4b>(bottomIndex + kj, kh).Item0);
                                    }
                                    if (newResult.Get<Vec4b>(kj, ki).Item1 + newImg.Get<Vec4b>(bottomIndex + kj, kh).Item1 > 255)
                                    {
                                        colorNew.Item1 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item1 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item1 + newImg.Get<Vec4b>(bottomIndex + kj, kh).Item1);
                                    }
                                    if (newResult.Get<Vec4b>(kj, ki).Item2 + newImg.Get<Vec4b>(bottomIndex + kj, kh).Item2 > 255)
                                    {
                                        colorNew.Item2 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item2 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item2 + newImg.Get<Vec4b>(bottomIndex + kj, kh).Item2);
                                    }
                                    if (newResult.Get<Vec4b>(kj, ki).Item3 + newImg.Get<Vec4b>(bottomIndex + kj, kh).Item3 > 255)
                                    {
                                        colorNew.Item3 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item3 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item3 + newImg.Get<Vec4b>(bottomIndex + kj, kh).Item3);
                                    }
                                }
                                else
                                {
                                    colorNew.Item0 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item0);
                                    colorNew.Item1 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item1);
                                    colorNew.Item2 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item2);
                                    colorNew.Item3 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item3);
                                }
                                newImg.Set(bottomIndex + kj, kh, colorNew);
                            }
                            km++;
                            kh++;
                        }
                    }
                }
                stringBuilder.Clear();
            }
            
            newImg?.SaveImage(@"D:\3D\Images\7-15\Output\1-1\OutPut.bmp");
            //2-1部分
            string imgPath2_1 = @"D:\3D\Images\7-15\2-2\2-2-";
            string newImgPath2_1 = @"D:\3D\Images\7-15\Output\2-2\2-2-";
            for (int k = 2; k <= 17; k++)
            {
                int m = 0;
                stringBuilder.Append(imgPath2_1 + k + ".bmp");
                Console.WriteLine(stringBuilder);
                using (Mat img = new Mat(Convert.ToString(stringBuilder), ImreadModes.AnyColor | ImreadModes.AnyDepth))
                using (Mat dst = new Mat())
                {
                    Mat result = new Mat();
                    Mat img_1 = new Mat();
                    Mat newResult = new Mat();
                    img.CopyTo(newResult);
                    Console.WriteLine("Channels=" + newResult.Channels());

                    for (int i = newResult.Rows - heightN; i < newResult.Rows; i++)
                    {
                        dataWeightHeight[m] = i;
                        Console.WriteLine("i=" + dataWeightHeight[m]);
                        m++;
                    }
                    for (int i = 0; i < dataWeightHeight.Length; i++)
                    {
                        Console.WriteLine("dataWeight=" + dataWeightHeight[i]);
                    }
                    float dataMax = dataWeightHeight.Max();
                    float dataMin = dataWeightHeight.Min();
                    for (int i = 0; i < dataWeightHeight.Length; i++)
                    {
                        newWeightHeight[i] = (dataWeightHeight[i] - dataMin) / (dataMax - dataMin);
                        Console.WriteLine("newWeight=" + newWeightHeight[i]);
                    }
                    //newResult.SaveImage(newImgPath + k + "_1.bmp");
                    m = 0;
                    for (int i = newResult.Rows - heightN; i < newResult.Rows; i++)
                    {
                        for (int j = 0; j < newResult.Cols; j++)
                        {
                            float f = 0;
                            if (newWeightHeight[m] >= 0 && newWeightHeight[m] <= 0.5)
                            {
                                f = Convert.ToSingle(0.5 * Math.Pow(2 * newWeightHeight[m], 2));

                            }
                            else if (newWeightHeight[m] >= 0.5 && newWeightHeight[m] <= 1)
                            {
                                f = Convert.ToSingle(1 - (1 - 0.5) * Math.Pow(2 * (1 - newWeightHeight[m]), 2));
                            }
                            Vec4b colorA = new Vec4b
                            {
                                Item0 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item0 * (1 - f)),
                                Item1 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item1 * (1 - f)),
                                Item2 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item2 * (1 - f)),
                                Item3 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item3 * (1 - f))
                            };
                            newResult.Set(i, j, colorA);
                        }
                        m++;
                    }

                    
                    for (int i = 0; i < newResult.Cols; i++)
                    {
                        m = 0;
                        for (int j = 0; j < heightN; j++)
                        {
                            float f = 0;
                            if (newWeightHeight[m] >= 0 && newWeightHeight[m] <= 0.5)
                            {
                                f = Convert.ToSingle(0.5 * Math.Pow(2 * newWeightHeight[m], 2));

                            }
                            else if (newWeightHeight[m] >= 0.5 && newWeightHeight[m] <= 1)
                            {
                                f = Convert.ToSingle(1 - (1 - 0.5) * Math.Pow(2 * (1 - newWeightHeight[m]), 2));
                            }
                            Vec4b colorA = new Vec4b
                            {
                                Item0 = Convert.ToByte(newResult.Get<Vec4b>(j ,i).Item0 * f),
                                Item1 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item1 * f),
                                Item2 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item2 * f),
                                Item3 = Convert.ToByte(newResult.Get<Vec4b>(j, i).Item3 * f)
                            };
                            newResult.Set(j, i, colorA);
                            m++;
                        }
                        
                    }

                    newResult.SaveImage(newImgPath2_1 + k + ".bmp");
                    if (k == 1)
                    {
                        for (int ki = 0; ki < newResult.Cols; ki++)
                        {
                            for (int kj = 0; kj < newResult.Rows; kj++)
                            {
                                Vec4b colorNew = new Vec4b
                                {
                                    Item0 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item0),
                                    Item1 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item1),
                                    Item2 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item2),
                                    Item3 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item3)
                                };
                                newImg.Set(kj, ki, colorNew);
                            }
                        }
                    }
                    else
                    {
                        int km = 0, kt = heightN, kWidth = 104;
                        int testKh = (k - 1) * kSubstract - (k - 1) * kt;
                        Console.WriteLine("kh = " + testKh);
                        for (int ki = 0; ki < newResult.Cols; ki++)
                        {
                            km = 0;
                            for (int kj = 0, kh = (k - 1) * kSubstract - (k - 1) * kt; kj < newResult.Rows; kj++)
                            {
                                Vec4b colorNew = newResult.Get<Vec4b>(kj, ki);
                                if (km < heightN)
                                {
                                    if (newResult.Get<Vec4b>(kj, ki).Item0 + newImg.Get<Vec4b>(kh + kj, (newImg.Cols - kWidth) + ki).Item0 > 255)
                                    {
                                        colorNew.Item0 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item0 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item0 + newImg.Get<Vec4b>(kh + kj, (newImg.Cols - kWidth) + ki).Item0);
                                    }
                                    if (newResult.Get<Vec4b>(kj, ki).Item1 + newImg.Get<Vec4b>(kh + kj, (newImg.Cols - kWidth) + ki).Item1 > 255)
                                    {
                                        colorNew.Item1 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item1 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item1 + newImg.Get<Vec4b>(kh + kj, (newImg.Cols - kWidth) + ki).Item1);
                                    }
                                    if (newResult.Get<Vec4b>(kj, ki).Item2 + newImg.Get<Vec4b>(kh + kj, (newImg.Cols - kWidth) + ki).Item2 > 255)
                                    {
                                        colorNew.Item2 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item2 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item2 + newImg.Get<Vec4b>(kh + kj, (newImg.Cols - kWidth) + ki).Item2);
                                    }
                                    if (newResult.Get<Vec4b>(kj, ki).Item3 + newImg.Get<Vec4b>(kh + kj, (newImg.Cols - kWidth) + ki).Item3 > 255)
                                    {
                                        colorNew.Item3 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item3 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item3 + newImg.Get<Vec4b>(kh + kj, (newImg.Cols - kWidth) + ki).Item3);
                                    }
                                }
                                else
                                {
                                    colorNew.Item0 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item0);
                                    colorNew.Item1 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item1);
                                    colorNew.Item2 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item2);
                                    colorNew.Item3 = Convert.ToByte(newResult.Get<Vec4b>(kj, ki).Item3);
                                }
                                newImg.Set(kh + kj, (newImg.Cols - kWidth) + ki, colorNew);
                            }
                            km++;
                            
                        }
                    }
                }
                stringBuilder.Clear();
            }
            newImg?.SaveImage(@"D:\3D\Images\7-15\Output\2-2\OutPut.bmp");


            //2-2部分
            string imgPath2_2 = @"D:\3D\Images\7-15\2-1\2-1-";
            string newImgPath2_2 = @"D:\3D\Images\7-15\Output\2-1\2-1-";
            for (int k = 2; k <= 17; k++)
            {
                int m = 0;
                stringBuilder.Append(imgPath2_2 + k + ".bmp");
                Console.WriteLine(stringBuilder);
                using (Mat img = new Mat(Convert.ToString(stringBuilder), ImreadModes.AnyColor | ImreadModes.AnyDepth))
                using (Mat dst = new Mat())
                {
                    Mat result = new Mat();
                    Mat img_1 = new Mat();
                    Mat newResult = new Mat();
                    img.CopyTo(newResult);
                    Console.WriteLine("Channels=" + newResult.Channels());

                    for (int i = newResult.Rows - heightN; i < newResult.Rows; i++)
                    {
                        dataWeightHeight[m] = i;
                        Console.WriteLine("i=" + dataWeightHeight[m]);
                        m++;
                    }
                    for (int i = 0; i < dataWeightHeight.Length; i++)
                    {
                        Console.WriteLine("dataWeight=" + dataWeightHeight[i]);
                    }
                    float dataMax = dataWeightHeight.Max();
                    float dataMin = dataWeightHeight.Min();
                    for (int i = 0; i < dataWeightHeight.Length; i++)
                    {
                        newWeightHeight[i] = (dataWeightHeight[i] - dataMin) / (dataMax - dataMin);
                        Console.WriteLine("newWeight=" + newWeightHeight[i]);
                    }
                    //newResult.SaveImage(newImgPath + k + "_1.bmp");
                    m = 0;
                    for (int i = newResult.Rows - heightN; i < newResult.Rows; i++)
                    {
                        for (int j = 0; j < newResult.Cols; j++)
                        {
                            float f = 0;
                            if (newWeightHeight[m] >= 0 && newWeightHeight[m] <= 0.5)
                            {
                                f = Convert.ToSingle(0.5 * Math.Pow(2 * newWeightHeight[m], 2));

                            }
                            else if (newWeightHeight[m] >= 0.5 && newWeightHeight[m] <= 1)
                            {
                                f = Convert.ToSingle(1 - (1 - 0.5) * Math.Pow(2 * (1 - newWeightHeight[m]), 2));
                            }
                            Vec4b colorA = new Vec4b
                            {
                                Item0 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item0 * (1 - f)),
                                Item1 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item1 * (1 - f)),
                                Item2 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item2 * (1 - f)),
                                Item3 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item3 * (1 - f))
                            };
                            newResult.Set(i, j, colorA);
                        }
                        m++;
                    }
                    
                    m = 0;
                    for (int i = 0; i < heightN; i++)
                    {
                        for (int j = 0; j < newResult.Cols; j++)
                        {
                            float f = 0;
                            if (newWeightHeight[m] >= 0 && newWeightHeight[m] <= 0.5)
                            {
                                f = Convert.ToSingle(0.5 * Math.Pow(2 * newWeightHeight[m], 2));

                            }
                            else if (newWeightHeight[m] >= 0.5 && newWeightHeight[m] <= 1)
                            {
                                f = Convert.ToSingle(1 - (1 - 0.5) * Math.Pow(2 * (1 - newWeightHeight[m]), 2));
                            }
                            Vec4b colorA = new Vec4b
                            {
                                Item0 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item0 * f),
                                Item1 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item1 * f),
                                Item2 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item2 * f),
                                Item3 = Convert.ToByte(newResult.Get<Vec4b>(i, j).Item3 * f)
                            };
                            newResult.Set(i, j, colorA);
                        }
                        m++;
                    }

                    newResult.SaveImage(newImgPath2_2 + k + ".bmp");
                    if (k == 1)
                    {
                        for (int ki = 0; ki < newResult.Rows; ki++)
                        {
                            for (int kj = 0; kj < newResult.Cols; kj++)
                            {
                                Vec4b colorNew = new Vec4b
                                {
                                    Item0 = Convert.ToByte(newResult.Get<Vec4b>(ki, kj).Item0),
                                    Item1 = Convert.ToByte(newResult.Get<Vec4b>(ki, kj).Item1),
                                    Item2 = Convert.ToByte(newResult.Get<Vec4b>(ki, kj).Item2),
                                    Item3 = Convert.ToByte(newResult.Get<Vec4b>(ki, kj).Item3)
                                };
                                newImg.Set(ki, kj, colorNew);
                            }
                        }
                    }
                    else
                    {
                        int km = 0, kt = heightN;
                        int testKh = (k - 1) * kSubstract - (k - 1) * kt;
                        Console.WriteLine("kh = " + testKh);
                        for (int ki = 0, kh = (k - 1) * kSubstract - (k - 1) * kt; ki < newResult.Rows; ki++)
                        {
                            
                            for (int kj = 0; kj < newResult.Cols; kj++)
                            {
                                Vec4b colorNew = newResult.Get<Vec4b>(ki, kj);
                                if (km < 10)
                                {
                                    if (newResult.Get<Vec4b>(ki, kj).Item0 + newImg.Get<Vec4b>(kh + ki, kj).Item0 > 255)
                                    {
                                        colorNew.Item0 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item0 = Convert.ToByte(newResult.Get<Vec4b>(ki, kj).Item0 + newImg.Get<Vec4b>(kh + ki, kj).Item0);
                                    }
                                    if (newResult.Get<Vec4b>(ki, kj).Item1 + newImg.Get<Vec4b>(kh + ki, kj).Item1 > 255)
                                    {
                                        colorNew.Item1 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item1 = Convert.ToByte(newResult.Get<Vec4b>(ki, kj).Item1 + newImg.Get<Vec4b>(kh + ki, kj).Item1);
                                    }
                                    if (newResult.Get<Vec4b>(ki, kj).Item2 + newImg.Get<Vec4b>(kh + ki, kj).Item2 > 255)
                                    {
                                        colorNew.Item2 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item2 = Convert.ToByte(newResult.Get<Vec4b>(ki, kj).Item2 + newImg.Get<Vec4b>(kh + ki, kj).Item2);
                                    }
                                    if (newResult.Get<Vec4b>(ki, kj).Item3 + newImg.Get<Vec4b>(kh + ki, kj).Item3 > 255)
                                    {
                                        colorNew.Item3 = 255;
                                    }
                                    else
                                    {
                                        colorNew.Item3 = Convert.ToByte(newResult.Get<Vec4b>(ki, kj).Item3 + newImg.Get<Vec4b>(kh + ki, kj).Item3);
                                    }
                                }
                                else
                                {
                                    colorNew.Item0 = Convert.ToByte(newResult.Get<Vec4b>(ki, kj).Item0);
                                    colorNew.Item1 = Convert.ToByte(newResult.Get<Vec4b>(ki, kj).Item1);
                                    colorNew.Item2 = Convert.ToByte(newResult.Get<Vec4b>(ki, kj).Item2);
                                    colorNew.Item3 = Convert.ToByte(newResult.Get<Vec4b>(ki, kj).Item3);
                                }
                                newImg.Set(kh + ki, kj, colorNew);
                            }
                            km++;
                            //kh++;
                        }
                    }
                }
                stringBuilder.Clear();
            }
            
            newImg?.SaveImage(@"D:\3D\Images\7-15\Output\2-1\OutPut.bmp");
        }

        #region 图像子块
        private void subImag_Click(object sender, EventArgs e)
        {
            
            int k = 145;
            while(k <= 235)
            {
                string SubImage = @"D:\3D\Images\ImageName\Image_" + k + @"\";
                int ImagePosition = 0;
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 16; col++)
                    {
                        Scalar scalar = new Scalar(0);
                        using (Mat src = new Mat(16, 9, MatType.CV_8UC1, scalar))
                        {
                            Cv2.Resize(src, src, new OpenCvSharp.Size(1920, 1080));
                            for (int i = 120 * row; i < (row + 1) * 120; i++)
                            {
                                for (int j = 120 * col; j < (col + 1) * 120; j++)
                                {
                                    src.Set(i, j, k);
                                }
                            }
                            ImagePosition++;
                            src?.SaveImage(SubImage + ImagePosition + ".bmp");
                        }

                    }

                }
                k = k + 10;
            }
            
            //for(int i = 0; i < 120; i++)
            //{
            //    for(int j = 0; j < 120; j++)
            //    {
            //        src.Set(i, j, 255);
            //    }
            //}
            //src?.SaveImage(SubImage);
        }
        #endregion

        #region 获取文件名称
        private  void BtnFileName_Click(object sender, EventArgs e)
        {
            DirectoryInfo RootFileName = new DirectoryInfo(@"D:/3D/Images/ImageName/Image_245");
            string[] _FileName = new string[RootFileName.GetFiles().Length];
            int k = 0;
            foreach (FileInfo file in RootFileName.GetFiles())
            {
                Console.WriteLine($"SubFileName={file.Name}");
                _FileName[k] = file.Name;
                k++;
            }
            Array.Sort(_FileName, new FileNameSort());
            for(int i = 0; i < _FileName.Length; i++)
            {
                Console.WriteLine($"FIle-{i}={_FileName[i]}");
            }
        }

        internal class FileNameSort : IComparer
        {
            //调用Windows的DLL
            [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
            private static extern int StrCmpLogicalW(string param1, string param2);
            //前后文件名进行比较
            public int Compare(object x, object y)
            {
                if(x == null && y == null)
                {
                    return 0;
                }
                if(x == null)
                {
                    return -1;
                }
                if(y == null)
                {
                    return 1;
                }
                return StrCmpLogicalW(x.ToString(), y.ToString());
            }
        }
        #endregion
        #region 图像清晰度评价
        private void clarifyEvalution_Click(object sender, EventArgs e)
        {

            double meanValue = 0;
            for(int i = 1; i < 61; i++)
            {
                using (Mat src = new Mat(@"D:/tupian/2/" + i + ".bmp"))
                {

                    Mat dst = new Mat();
                    Cv2.Laplacian(src, dst, src.Type());
                    meanValue = Cv2.Mean(dst)[0];
                    Console.WriteLine(i + "清晰度值为：" + meanValue);
                }
            }
            
        }
        #endregion
    }
}
