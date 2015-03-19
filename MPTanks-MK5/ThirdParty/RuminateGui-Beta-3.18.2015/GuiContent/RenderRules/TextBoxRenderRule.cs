using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ruminate.GUI.Framework;
using Ruminate.Utils;

namespace Ruminate.GUI.Content {

    public class TextBoxRenderRule : FontRenderRule {

        private Rectangle _area;
        public override Rectangle Area {
            get { return _area; } 
            set { _area = value; }
        }

        public void RecreateStringData(int maxChars) {
            _maxLength = maxChars;

            _textCursor = 0;
            _selectedChar = null;

            _char = new char[MaxLength];
            _charX = new short[MaxLength];
            _charY = new short[MaxLength];
            _charWidth = new byte[MaxLength];
            _row = new byte[MaxLength];
        }

        public override void SetSize(int w, int h) {
            _area.Width = w;
            _area.Height = h;
        }

        protected override void LoadRenderers() {
            Cursor = LoadRenderer<IconRenderer>(Skin, "cursor");
            
            MeasureText();            
        }

        public override void Draw() {
            if (RenderedText != null) { RenderManager.SpriteBatch.Draw(RenderedText, Area, Color.White); }
            if (CursorVisible) { RenderCursor(RenderManager.SpriteBatch, Area); }
            RenderSelection(Area);
        }

        public override void DrawNoClipping() { }

        public IconRenderer Cursor { get; set; }

        public bool CursorVisible { get; set; }

        /*####################################################################*/
        /*                             Variables                              */
        /*####################################################################*/
        
        private int _textCursor;
        private int? _selectedChar;

        private int _maxLength;
        private char[] _char;
        private short[] _charX, _charY;
        private byte[] _charWidth, _row;

        private Texture2D RenderedText { get; set; }

        /*####################################################################*/
        /*                         Input Filtering                            */
        /*####################################################################*/

        /// <summary>
        /// The current location of the cursor in the array
        /// </summary>
        public int TextCursor {
            get {
                return _textCursor;
            } set {
                _textCursor = Clamp(value, 0, Length);
            }
        }

        /// <summary>
        /// All characters between SelectedChar and the TextCursor are selected 
        /// when SelectedChar != null. Cannot be the same as the TextCursor value.
        /// </summary>
        public int? SelectedChar {
            get {
                return _selectedChar;
            } set {
                if (value.HasValue) {
                    if (value.Value != TextCursor) {
                        _selectedChar = (short)Clamp(value.Value, 0, Length);
                    }
                } else {
                    _selectedChar = null;
                }
            }
        }

        /// <summary>
        /// Current number of character in the textbox
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Maximum number of character in the textbox
        /// </summary>
        public int MaxLength { get { return _maxLength; } }

        /// <summary>
        /// The current characters in the textbox in string format
        /// </summary>
        public string Value {
            get {
                return new string(_char).Substring(0, Length);
            } set {
                SetText(value);
                if (!Loaded) { return; }
                MeasureText();
                BakeText();
            }
        }

        /// <summary>
        /// Returns true when there is selected text
        /// </summary>
        public bool HasSelected { get { return SelectedChar.HasValue && SelectedChar.Value != TextCursor; } }

        /*####################################################################*/
        /*                         Value Alteration                           */
        /*####################################################################*/

        /// <summary>
        /// Replace the text in the textbox with the specified string.
        /// </summary>
        /// <param name="value">A string that overrides the current text of the textbox.</param>
        public void SetText(string value) {            

            var x = value.IndexOf('\0');
            if (x != -1) { value = value.Substring(0, x); }

            Length = value.Length;

            TextCursor = Length;
            SelectedChar = null;

            Array.Clear(_char, 0, _char.Length);

            value.ToCharArray().CopyTo(_char, 0);            
        }

        public void MeasureText() {
            for (var i = 0; i < Length; i++) {
                _charWidth[i] = MeasureCharacter(i);
            }
        }

        /// <summary>
        /// Insert an individual character after the cursor then move the 
        /// cursor forward one.
        /// </summary>
        /// <param name="character">The character to insert. Will not be inserted if the 
        /// textbox is full or if the character is not supported by the current font.</param>
        public void Insert(char character) {

            if (!Font.Characters.Contains(character) && character != '\r' && character != '\n') { return; }
            if (!(Length < MaxLength)) { return; }

            //Shift everything right once then insert the character into the gap
            Array.Copy(
                _char, TextCursor,
                _char, TextCursor + 1,
                Length - TextCursor);
            Array.Copy(
                _charWidth, TextCursor,
                _charWidth, TextCursor + 1,
                Length - TextCursor);

            _char[TextCursor] = character;
            var size = MeasureCharacter(TextCursor);
            _charWidth[TextCursor] = size;

            Length++;

            //Cursor needs to shift right
            TextCursor++;
        }

        /// <summary>
        /// Insert a string into the textbox at the cursor's location.
        /// </summary>
        /// <param name="value">The string to insert.</param>
        public void Insert(string value) {
            foreach (var character in value) {
                Insert(character);
            }
        }

        /// <summary>
        /// Remove the character to the right of the cursor. If the cursor is after 
        /// the last character do nothing.
        /// </summary>
        public void Delete() {

            if (HasSelected) {
                RemoveSelected();
                return;
            }

            if (Length == 0 || TextCursor == Length) { return; }

            var start = TextCursor + 1;
            Array.Copy(_char, start, _char, TextCursor, Length - start);
            Array.Copy(_charWidth, start, _charWidth, TextCursor, Length - start);

            Length--;
            Remeasure();
        }

        /// <summary>
        /// Remove the character before the cursor. If the cursor is before the 
        /// first character do nothing.
        /// </summary>
        public void BackSpace() {

            if (TextCursor == 0) { return; }
            TextCursor--;
            Delete();
        }

        /// <summary>
        /// Removes the currently highlighted text.
        /// </summary>
        public void RemoveSelected() {

            if (!HasSelected) { return; }

            var range = GetSelectionRange();

            Value = Value.Remove(range.X, range.Y - range.X);
            TextCursor = range.X;

            SelectedChar = null;

            if (TextCursor >= range.Y && TextCursor <= range.Y) {
                TextCursor = range.X;
            }
        }

        /*####################################################################*/
        /*                             Selection                              */
        /*####################################################################*/

        //Test[x]
        public void CursorUp() {

            if (TextCursor == Length && (_char[TextCursor - 1] == '\n' || _char[TextCursor - 1] == '\r')) {
                TextCursor--;
            } else if (TextCursor == Length) {

                if (_row[TextCursor - 1] - 1 < 0) {
                    TextCursor = 0;
                    return;
                }

                var location = TextCursor - 1;

                while (location >= 0) {
                    if (_row[location] == _row[TextCursor - 1] - 1 && _charX[location] <= _charX[TextCursor - 1]) {
                        TextCursor = location;
                        return;
                    }
                    location--;
                }
            } else if (_row[TextCursor] == 0) {
                TextCursor = 0;
            } else {
                var location = TextCursor;

                while (location >= 0) {
                    if (_row[location] == _row[TextCursor] - 1 && _charX[location] <= _charX[TextCursor]) {
                        TextCursor = location;
                        return;
                    }
                    location--;
                }
            }
        }

        public void CursorDown() {

            var start = TextCursor == Length ? TextCursor - 1 : TextCursor;
            var location = TextCursor == Length ? TextCursor - 1 : TextCursor;

            while (location <= Length - 1) {
                if (_row[location] == _row[start] + 1 && _charX[location] >= _charX[start]) {
                    TextCursor = location;
                    return;
                }
                if (_row[location] == _row[start] + 2) {
                    TextCursor = location - 1;
                    return;
                }
                location++;
            }

            TextCursor = Length;
        }

        public void CursorHome() {

            var homeCursor = (TextCursor == MaxLength) ? TextCursor - 1 : TextCursor;

            var homeRow = _row[homeCursor];
            var homeStart = homeCursor;

            TextCursor = 0;

            for (var i = homeStart; i > 0; i--) {
                if (_row[i] >= homeRow) { continue; }
                TextCursor = i + 1;
                break;
            }
        }

        public void CursorEnd() {

            var endCursor = (TextCursor == MaxLength) ? TextCursor - 1 : TextCursor;

            var endRow = _row[endCursor];
            var endStart = endCursor;

            //Jump to end of text by default
            TextCursor = Length;

            //Jump to end of line if possible. Will fall though if not possible
            for (var i = endStart; i < Length; i++) {
                if (_row[i] <= endRow) { continue; }
                TextCursor = i - 1;
                break;
            }
        }

        public void SetSelection(Point localLocation) {
            SelectedChar = CharAt(localLocation);
        }

        public void SetTextCursor(Point localLocation) {
            TextCursor = CharAt(localLocation);
        }

        private int CharAt(Point localLocation) {

            var charRectangle = new Rectangle(0, 0, 0, Font.LineSpacing);

            var row = localLocation.Y / (Font.LineSpacing);

            for (short i = 0; i < Length; i++) {

                if (_row[i] != row) { continue; }

                //Rectangle that encompasses the current character
                charRectangle.X = _charX[i];
                charRectangle.Y = _charY[i];
                charRectangle.Width = _charWidth[i];

                //Click on a character so put the cursor in front of it
                if (charRectangle.Contains(localLocation)) { return i; }

                //Next character is not on the correct row so this is the last character for this row so select it.
                if (i < Length - 1 && _row[i + 1] != row) { return i; }
            }

            //Missed a character so return the end.
            return Length;
        }

        public Point GetSelectionRange() {

            if (!SelectedChar.HasValue) { return Point.Zero; }
            var selected = SelectedChar.Value;

            return new Point(
                (selected < TextCursor) ? selected : TextCursor,
                (selected < TextCursor) ? TextCursor : selected
            );
        }

        public string GetSelected() {

            if (!HasSelected) { return String.Empty; }

            var range = GetSelectionRange();

            return Value.Substring(range.X, range.Y - range.X);
        }

        /*####################################################################*/
        /*                              Helpers                               */
        /*####################################################################*/

        private static int Clamp(int value, int min, int max) {
            if (value > max) return max;
            else if (value < min) return min;
            else return value;
        }

        private byte MeasureCharacter(int location) {

            var value = new string(_char);
            var front = Font.MeasureString(value.Substring(0, location)).X;
            var end = Font.MeasureString(value.Substring(0, location + 1)).X;

            return (byte)(end - front);
        }

        private void Remeasure() {
            for (var i = 0; i < Length; i++) {
                _charWidth[i] = MeasureCharacter(i);
            }
        }

        /*####################################################################*/
        /*                             Rendering                              */
        /*####################################################################*/

        public void RenderCursor(SpriteBatch batch, Rectangle renderArea) {

            //Top left corner of the text area
            int x = renderArea.X,
                y = renderArea.Y;

            if (TextCursor > 0) {
                if (_char[TextCursor - 1] == '\n' || _char[TextCursor - 1] == '\r') {
                    //Beginning of next line
                    y += _charY[TextCursor - 1] + Font.LineSpacing;
                } else if (TextCursor == Length) {
                    //After last character
                    x += _charX[TextCursor - 1] + _charWidth[TextCursor - 1];
                    y += _charY[TextCursor - 1];
                } else {
                    //Beginning of current character                
                    x += _charX[TextCursor];
                    y += _charY[TextCursor];
                }
            }
            Cursor.Render(batch, new Rectangle(x - 1, y, Cursor.Size.X, Font.LineSpacing));
        }

        public void RenderSelection(Rectangle renderArea) {

            if (!HasSelected) { return; }

            var rectangle = new Rectangle(0, 0, 0, Font.LineSpacing);

            var start = (SelectedChar.Value < TextCursor) ? SelectedChar.Value : TextCursor;
            var end = (SelectedChar.Value > TextCursor) ? SelectedChar.Value : TextCursor;

            for (var i = start; i < end; i++) {
                //Rectangle that encompasses the current character
                rectangle.X = _charX[i] + renderArea.X;
                rectangle.Y = _charY[i] + renderArea.Y;
                rectangle.Width = _charWidth[i];

                RenderManager.SpriteBatch.Draw(RenderManager.SelectionColor, rectangle, Color.White);
            }
        }

        public void BakeText() {

            var textureBaker = new TextureBaker(
                RenderManager.GraphicsDevice,
                Area.Width,
                Area.Height,
                TextureBaker.RenderState.Fill);

            var start = 0;
            var height = 0.0f;

            while (true) {

                start = RenderLine(textureBaker, start, height);

                if (start >= Length) {
                    RenderedText = textureBaker.GetTexture();
                    return;
                }

                height += Font.LineSpacing;
            }
        }

        private int RenderLine(SpriteBatch textureBaker, int start, float height) {

            var breakLocation = start;
            var lineLength = 0.0f;
            var row = (byte)(height / Font.LineSpacing);
            string text = new string(_char), tempText;

            //Starting from end of last line loop though the characters
            for (var iCount = start; iCount < Length; iCount++) {

                //Calculate screen location of current character
                _charX[iCount] = (short)lineLength;
                _charY[iCount] = (short)height;
                _row[iCount] = row;

                //Calculate the width of the current line
                lineLength += _charWidth[iCount];

                //Current line is too long need to split it
                if (lineLength > Area.Width) {
                    if (breakLocation == start) {
                        //Have to split a word
                        //Render line and return start of new line
                        tempText = text.Substring(start, iCount - start);
                        textureBaker.DrawString(Font, tempText, new Vector2(0.0f, height), TextRenderer.Color);
                        return iCount + 1;
                    } else {
                        //Have a character we can split on
                        //Render line and return start of new line
                        tempText = text.Substring(start, breakLocation - start);
                        textureBaker.DrawString(Font, tempText, new Vector2(0.0f, height), TextRenderer.Color);
                        return breakLocation + 1;
                    }
                }

                //Handle characters that force/allow for breaks
                switch (_char[iCount]) {
                    //These characters force a line break
                    case '\r':
                    case '\n':
                        //Render line and return start of new line
                        tempText = text.Substring(start, iCount - start);
                        textureBaker.DrawString(Font, tempText, new Vector2(0.0f, height), TextRenderer.Color);
                        return iCount + 1;
                    //These characters are good break locations
                    case '-':
                    case ' ':
                        breakLocation = iCount + 1;
                        break;
                }
            }

            //We hit the end of the text box render line and return
            //_textData.Length so RenderText knows to return
            tempText = text.Substring(start, Length - start);
            textureBaker.DrawString(Font, tempText, new Vector2(0.0f, height), TextRenderer.Color);
            return Length;
        }        
    }
}