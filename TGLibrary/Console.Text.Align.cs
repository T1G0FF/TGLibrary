using System;

namespace TGLibrary {
    namespace TGConsole {
        /// <remarks>
        /// Contains methods for manipulating text.
        /// </remarks>
        public partial class Text {
            public enum Align { Left, Center, Right };

            #region Left
            public static string Left(int totalWidth, string text) {
                return RowBuilder(totalWidth, ' ', ' ', text, ' ', ' ', Align.Left);
            }

            public static string Left(int totalWidth, string text, char ends) {
                return RowBuilder(totalWidth, ends, ' ', text, ' ', ends, Align.Left);
            }

            public static string Left(int totalWidth, string text, char leftEnd, char rightEnd) {
                return RowBuilder(totalWidth, leftEnd, ' ', text, ' ', rightEnd, Align.Left);
            }

            public static string Left(int totalWidth, string text, char leftEnd, char line, char rightEnd) {
                return RowBuilder(totalWidth, leftEnd, line, text, line, rightEnd, Align.Left);
            }
            #endregion
            #region Center
            public static string Center(int totalWidth, string text) {
                return RowBuilder(totalWidth, ' ', ' ', text, ' ', ' ', Align.Center);
            }

            public static string Center(int totalWidth, string text, char ends) {
                return RowBuilder(totalWidth, ends, ' ', text, ' ', ends, Align.Center);
            }

            public static string Center(int totalWidth, string text, char leftEnd, char rightEnd) {
                return RowBuilder(totalWidth, leftEnd, ' ', text, ' ', rightEnd, Align.Center);
            }

            public static string Center(int totalWidth, string text, char leftEnd, char line, char rightEnd) {
                return RowBuilder(totalWidth, leftEnd, line, text, line, rightEnd, Align.Center);
            }
            #endregion
            #region Right
            public static string Right(int totalWidth, string text) {
                return RowBuilder(totalWidth, ' ', ' ', text, ' ', ' ', Align.Right);
            }

            public static string Right(int totalWidth, string text, char ends) {
                return RowBuilder(totalWidth, ends, ' ', text, ' ', ends, Align.Right);
            }

            public static string Right(int totalWidth, string text, char leftEnd, char rightEnd) {
                return RowBuilder(totalWidth, leftEnd, ' ', text, ' ', rightEnd, Align.Right);
            }

            public static string Right(int totalWidth, string text, char leftEnd, char line, char rightEnd) {
                return RowBuilder(totalWidth, leftEnd, line, text, line, rightEnd, Align.Right);
            }
            #endregion

            private static string RowBuilder(int totalWidth, char leftEnd, char leftSpacer, string row, char rightSpacer, char rightEnd, Align alignment) {
                string leftSide = "";
                string rightSide = "";

                switch (alignment) {
                    case Align.Left:
                        leftSide = " ";
                        rightSide = " " + new string(rightSpacer, (totalWidth - row.Length) - 4);
                        break;
                    case Align.Right:
                        leftSide = new string(leftSpacer, (totalWidth - row.Length) - 4) + " ";
                        rightSide = " ";
                        break;
                    default:
                    case Align.Center:
                        int halfWidth = (int)Math.Ceiling(totalWidth / 2D);
                        int halfLength = (int)Math.Ceiling(row.Length / 2D);
                        int startText;
                        int endText;

                        if (halfWidth <= halfLength) {
                            // 4 in total = 2 Ends and 2 Spaces
                            startText = 2;
                            endText = totalWidth - 2;
                        }
                        else {
                            startText = halfWidth - halfLength;
                            endText = startText + row.Length;
                        }

                        leftSide = new string(leftSpacer, startText - 2) + " ";
                        rightSide = " " + new string(rightSpacer, (totalWidth - endText) - 2);
                        break;
                }
                return (leftEnd + leftSide + row + rightSide + rightEnd);
            }
        }
    }
}
