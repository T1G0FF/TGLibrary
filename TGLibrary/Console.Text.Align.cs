using System;

namespace TGLibrary {
    namespace TGConsole {
        /// <remarks>
        /// Contains methods for manipulating text.
        /// </remarks>
        public partial class Text {
            private enum Align { Left, Center, Right };

            #region Left
            public static string Left(int width, string text) {
                return Left(width, text, ' ');
            }

            public static string Left(int width, string text, char ends) {
                return Left(width, text, ends, ends);
            }

            public static string Left(int width, string text, char leftEnd, char rightEnd) {
                return RowBuilder(width, leftEnd, ' ', text, ' ', rightEnd, Align.Left);
            }

            public static string Left(int width, string text, char leftEnd, char line, char rightEnd) {
                return RowBuilder(width, leftEnd, line, text, line, rightEnd, Align.Left);
            }
            #endregion
            #region Center
            public static string Center(int width, string text) {
                return Center(width, text, ' ');
            }

            public static string Center(int width, string text, char ends) {
                return Center(width, text, ends, ends);
            }

            public static string Center(int width, string text, char leftEnd, char rightEnd) {
                return RowBuilder(width, leftEnd, ' ', text, ' ', rightEnd, Align.Center);
            }

            public static string Center(int width, string text, char leftEnd, char line, char rightEnd) {
                return RowBuilder(width, leftEnd, line, text, line, rightEnd, Align.Center);
            }
            #endregion
            #region Right
            public static string Right(int width, string text) {
                return Right(width, text, ' ');
            }

            public static string Right(int width, string text, char ends) {
                return Right(width, text, ends, ends);
            }

            public static string Right(int width, string text, char leftEnd, char rightEnd) {
                return RowBuilder(width, leftEnd, ' ', text, ' ', rightEnd, Align.Right);
            }

            public static string Right(int width, string text, char leftEnd, char line, char rightEnd) {
                return RowBuilder(width, leftEnd, line, text, line, rightEnd, Align.Right);
            }
            #endregion

            private static string RowBuilder(int width, char leftEnd, char leftSpacer, string row, char rightSpacer, char rightEnd, Align alignment) {
                string leftSide = "";
                string rightSide = "";

                switch (alignment) {
                    case Align.Left:
                        leftSide = " ";
                        rightSide = " " + new string(rightSpacer, (width - row.Length) - 4);
                        break;
                    case Align.Right:
                        leftSide = new string(leftSpacer, (width - row.Length) - 4) + " ";
                        rightSide = " ";
                        break;
                    default:
                    case Align.Center:
                        int halfWidth = (int)Math.Ceiling(width / 2D);
                        int halfLength = (int)Math.Ceiling(row.Length / 2D);
                        int startText;
                        int endText;

                        if (halfWidth <= halfLength) {
                            // 4 in total = 2 Ends and 2 Spaces
                            startText = 2;
                            endText = width - 2;
                        }
                        else {
                            startText = halfWidth - halfLength;
                            endText = startText + row.Length;
                        }

                        leftSide = new string(leftSpacer, startText - 2) + " ";
                        rightSide = " " + new string(rightSpacer, (width - endText) - 2);
                        break;
                }
                return (leftEnd + leftSide + row + rightSide + rightEnd);
            }
        }
    }
}
