using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace TGLibrary {
	public static class BitmapHelpers {
		public static Bitmap CombineImages(Bitmap above, Bitmap below) {
			using (Graphics gph = Graphics.FromImage(below)) {
				gph.DrawImage(above, new Point(0, 0));
			}
			return below;
		}

		public static Bitmap CropImage(Bitmap img, Rectangle cropArea) {
			Bitmap crop = new Bitmap(cropArea.Width, cropArea.Height);
			using (Graphics gph = Graphics.FromImage(crop)) {
				gph.DrawImage(img, new Rectangle(0, 0, crop.Width, crop.Height), cropArea, GraphicsUnit.Pixel);
			}
			return crop;
		}

		public static Bitmap TextOverlay(Bitmap bmp, string text, string font = "Arial", int size = 12, Brush brush = null) {
			brush = brush ?? Brushes.Black;
			FontFamily tinyFamily = new FontFamily(font);
			Font tinyFont = new Font(tinyFamily, size);
			int ascent = tinyFamily.GetCellAscent(FontStyle.Regular);
			float ascentPixel = tinyFont.Size * ascent / tinyFamily.GetEmHeight(FontStyle.Regular);
			int descent = tinyFamily.GetCellDescent(FontStyle.Regular);
			float descentPixel = tinyFont.Size * descent / tinyFamily.GetEmHeight(FontStyle.Regular);
			descentPixel = (float) Math.Round(descentPixel, MidpointRounding.AwayFromZero);

			StringFormat sf = new StringFormat(StringFormat.GenericDefault) {
				LineAlignment = StringAlignment.Center,
				Alignment = StringAlignment.Center
			};

			using (Graphics gph = Graphics.FromImage(bmp)) {
				gph.SmoothingMode = SmoothingMode.HighQuality;
				gph.InterpolationMode = InterpolationMode.HighQualityBicubic;
				gph.PixelOffsetMode = PixelOffsetMode.HighQuality;
				gph.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

				PointF pointF = new PointF(bmp.Width / 2F, (2 * descentPixel) + 1);
				float lineHeight = tinyFont.Size * tinyFamily.GetLineSpacing(FontStyle.Regular) / tinyFamily.GetEmHeight(FontStyle.Regular);
				lineHeight = lineHeight < ascentPixel + descentPixel ? lineHeight : ascentPixel + descentPixel;
				string[] toWrite = text.Split(new char[] { '\n' });
				for (int i = 0; i < toWrite.Length; i++) {
					gph.DrawString(toWrite[i], tinyFont, brush, pointF, sf);
					pointF.Y += lineHeight;
				}
				gph.Flush();
			}

			return bmp;
		}
	}
}
