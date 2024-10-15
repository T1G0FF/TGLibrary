using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

namespace TGLibrary {
	public sealed class ZoomPictureBox : UserControl {
		#region Constructors
		public ZoomPictureBox() {
			this.DoubleBuffered = true;
			this.BackColor = Color.Black;
			this.Size = new Size(200, 200);
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Enable dragging. Set to False if you implement other means of image scrolling.
		/// </summary>
		public bool EnableMouseDragging { get; set; } = true;

		/// <summary>
		/// Enable mouse wheel zooming. Set to False e.g. if you control zooming with a TrackBar.
		/// </summary>
		public bool EnableMouseWheelZooming { get; set; } = true;

		/// <summary>
		/// Image to display in the ZoomPictureBox.
		/// </summary>
		public Image Image {
			get { return _image; }
			set {
				_image = value;
				if (value != null)
					InitializeImage();
				else
					_imageInitialized = false;
			}
		}

		/// <summary>
		/// The bounding rectangle of the zoomed image relative to the control origin.
		/// </summary>
		public Rectangle ImageBounds => _imageBounds;

		/// <summary>
		/// Location of the top left corner of the zoomed image relative to the control origin.
		/// </summary>
		public Point ImagePosition {
			get { return _imageBounds.Location; }
			set {
				this.Invalidate(_imageBounds);
				_imageBounds.X = value.X;
				_imageBounds.Y = value.Y;
				this.Invalidate(_imageBounds);
			}
		}

		/// <summary>
		/// The maximum zoom magnification.
		/// </summary>
		public double MaximumZoomFactor { get; set; } = 64;

		/// <summary>
		/// Minimum height of the zoomed image in pixels.
		/// </summary>
		public int MinimumImageHeight { get; set; } = 50;

		/// <summary>
		/// Minimum width of the zoomed image in pixels.
		/// </summary>
		public int MinimumImageWidth { get; set; } = 50;

		/// <summary>
		/// Sets the responsiveness of zooming to the mouse wheel. Choose a lower value for faster zooming.
		/// </summary>
		public int MouseWheelDivisor { get; set; } = 480;

		/// <summary>
		/// Linear size of the zoomed image as a fraction of that of the source Image.
		/// </summary>
		public double ZoomFactor {
			get { return _zoomFactor; }
			set {
				_zoomFactor = ValidateZoomFactor(value);
				if (_imageInitialized) {
					this.Invalidate(_imageBounds);
					_imageBounds = GetZoomedBounds();
					this.Invalidate(_imageBounds);
				}
			}
		}

		/// <summary>
		/// Image zooming around the mouse position, image center or control center.
		/// </summary>
		public ZoomLocation ZoomTo { get; set; } = ZoomLocation.MousePosition;
		#endregion

		#region Private Variables
		private Rectangle _imageBounds;
		private double _zoomFactor;
		private Image _image;
		private Point _startDrag;
		private bool _dragging;
		private bool _imageInitialized;
		private double _previousZoomFactor;
		private bool _scaleToggle = true;
		#endregion

		#region Enums
		public enum ZoomLocation {
			MousePosition,
			ControlCenter,
			ImageCenter,
		}
		#endregion

		#region Event Overrides
		protected override void OnMouseDown(MouseEventArgs e) {
			this.Select();
			switch (e.Button) {
				case MouseButtons.Left:
					if (EnableMouseDragging) {
						_startDrag = e.Location;
						_dragging = true;
					}
					break;
				case MouseButtons.Right:
					break;
				case MouseButtons.Middle:
					if (_scaleToggle) {
						ZoomFactor = 1.0F;
					}
					else {
						ZoomFactor = FitImageToControl();
					}
					_scaleToggle = !_scaleToggle;
					_imageBounds = CenterImageBounds();
					this.Invalidate();
					break;
				case MouseButtons.XButton1:
					break;
				case MouseButtons.XButton2:
					break;
				case MouseButtons.None:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			if (_dragging) {
				this.Invalidate(_imageBounds);
				_imageBounds.X += e.X - _startDrag.X;
				_imageBounds.Y += e.Y - _startDrag.Y;
				_startDrag = e.Location;
				this.Invalidate(_imageBounds);
			}
			base.OnMouseMove(e);
		}

		protected override void OnSizeChanged(EventArgs e) {
			this.Select();
			InitializeImage();
			base.OnSizeChanged(e);
		}

		protected override void OnMove(EventArgs e) {
			this.Select();
			base.OnMove(e);
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			if (_dragging) {
				_dragging = false;
				this.Invalidate();
			}
			base.OnMouseUp(e);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			switch (keyData) {
				case Keys.Control | Keys.S:
					SaveCurrentImage();
					return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		public void SaveCurrentImage() {
			Thread newThread = new Thread(saveCurrentImage) {
				IsBackground = true
			};
			newThread.SetApartmentState(ApartmentState.STA);
			newThread.Start();
		}

		private void saveCurrentImage() {
			using (FolderBrowserDialog dlg = new FolderBrowserDialog()) {
				DialogResult result = dlg.ShowDialog();
				if (result == DialogResult.OK) {
					string folderPath = dlg.SelectedPath + "\\";
					string filePath = folderPath + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".png";
					Image.Save(filePath, ImageFormat.Png);
				}
			}
		}

		// The control only raises MouseWheel events when it has Focus.
		protected override void OnMouseEnter(EventArgs e) {
			this.Select();
			base.OnMouseEnter(e);
		}

		// Mouse wheel zooming.
		protected override void OnMouseWheel(MouseEventArgs e) {
			if (EnableMouseWheelZooming &&
				this.ClientRectangle.Contains(e.Location)) {
				double zoom = _zoomFactor;
				zoom *= 1 + (double)e.Delta / MouseWheelDivisor;
				ZoomFactor = zoom;
			}
			base.OnMouseWheel(e);
		}

		// Render the image in the _ImageBounds rectangle
		protected override void OnPaint(PaintEventArgs pe) {
			if (_zoomFactor > 2) {
				pe.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
				pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			}
			else {
				pe.Graphics.InterpolationMode = InterpolationMode.Default;
			}
			if (_image != null) {
				pe.Graphics.DrawImage(_image, _imageBounds);
			}
			base.OnPaint(pe);
		}
		#endregion

		#region Private Methods
		private void InitializeImage() {
			if (_image != null) {
				ZoomFactor = FitImageToControl();
				_imageBounds = CenterImageBounds();
			}
			_imageInitialized = true;
			this.Invalidate();
		}

		// Apply the maximum and minimum zoom limits:
		private double ValidateZoomFactor(double zoom) {
			zoom = Math.Min(zoom, MaximumZoomFactor);
			if (this._image != null) {
				if ((int)(_image.Width * zoom) < MinimumImageWidth)
					zoom = (double)MinimumImageWidth / _image.Width;
				if ((int)(_image.Height * zoom) < MinimumImageHeight)
					zoom = (double)MinimumImageHeight / _image.Height;
			}
			return zoom;
		}

		// Get the initial ZoomFactor to fit the image to the control.
		private double FitImageToControl() {
			if (this.ClientSize == Size.Empty)
				return 1;

			double sourceAspect = (double) _image.Width / _image.Height;
			double targetAspect = (double) this.ClientSize.Width / this.ClientSize.Height;

			if (sourceAspect > targetAspect)
				return (double)this.ClientSize.Width / _image.Width;
			else
				return (double)this.ClientSize.Height / _image.Height;
		}

		// Center the zoomed image in the control bounds.
		private Rectangle CenterImageBounds() {
			int w = (int) (_image.Width * _zoomFactor);
			int h = (int) (_image.Height * _zoomFactor);
			int x = (this.ClientSize.Width - w) / 2;
			int y = (this.ClientSize.Height - h) / 2;
			return new Rectangle(x, y, w, h);
		}

		// Calculate the image bounds for a given ZoomFactor,
		private Rectangle GetZoomedBounds() {
			if (_image == null)
				return Rectangle.Empty;

			// Find the zoom center relative to the image bounds.
			Point imageCenter = FindZoomCenter(ZoomTo);

			// Calculate the new size of the the image bounds.
			_previousZoomFactor = (double)_imageBounds.Width / _image.Width;
			if (Math.Abs(_zoomFactor - _previousZoomFactor) > 0.001) {
				double zoomRatio = _zoomFactor / _previousZoomFactor;
				_imageBounds.Width = (int)(_imageBounds.Width * zoomRatio);
				_imageBounds.Height = (int)(_imageBounds.Height * zoomRatio);

				// Find the resulting position of the zoom center prior to correction.
				Point newPRelative = new Point {
					X = (int) (imageCenter.X * zoomRatio),
					Y = (int) (imageCenter.Y * zoomRatio)
				};

				// Apply a correction to return the zoom center to its previous position.
				_imageBounds.X += imageCenter.X - newPRelative.X;
				_imageBounds.Y += imageCenter.Y - newPRelative.Y;
			}
			_previousZoomFactor = _zoomFactor;
			return _imageBounds;
		}

		// Find the zoom centre relative to the image bounds, depending on zoom mode.
		private Point FindZoomCenter(ZoomLocation location) {
			Point p = new Point();
			switch (location) {
				case ZoomLocation.MousePosition:
					Point mp = this.PointToClient(MousePosition);
					p.X = mp.X - _imageBounds.X;
					p.Y = mp.Y - _imageBounds.Y;
					break;
				case ZoomLocation.ControlCenter:
					p.X = this.Width / 2 - _imageBounds.X;
					p.Y = this.Height / 2 - _imageBounds.Y;
					break;
				case ZoomLocation.ImageCenter:
					p.X = _imageBounds.Width / 2;
					p.Y = _imageBounds.Height / 2;
					break;
				default:
					p = Point.Empty;
					break;
			}
			return p;
		}
		#endregion
	}
}
