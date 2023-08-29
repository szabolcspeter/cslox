namespace lox
{
    public class Scanner
    {
        private readonly string _source;
        private readonly List<Token> _tokens = new ();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(string source)
        {
            _source = source;
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                // We are at the beginning of the next lexeme.
                start = current;
                ScanToken();
            }

            _tokens.Add(new Token(TokenType.EOF, "", null, line));
            return _tokens;
        }

        #region private methods

        private bool IsAtEnd()
        {
            return current >= _source.Length;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {   
                case '(':
                    AddToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    AddToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    AddToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    AddToken(TokenType.RIGHT_BRACE);
                    break;
                case ',':
                    AddToken(TokenType.COMMA);
                    break;
                case '.':
                    AddToken(TokenType.DOT);
                    break;
                case '-':
                    AddToken(TokenType.MINUS);
                    break;
                case '+':
                    AddToken(TokenType.PLUS);
                    break;
                case ';':
                    AddToken(TokenType.SEMICOLON);
                    break;
                case '*':
                    AddToken(TokenType.STAR);
                    break;
                // two-char-tokens
                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '/':
                    if (Match('/'))
                    {
                        // a comment goes until the end of the line.
                        while (Peek() != '\n' && !IsAtEnd())
                            Advance();
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // ignore whitespace characters
                    break;
                case '\n':
                    line++;
                    break;
                default:
                    Lox.Error(line, "Unexpected character.");
                    break;
            }
        }

        private char Peek()
        {
            if (IsAtEnd())
                return '\0';
            return _source[current];
        }

        private bool Match(char expected)
        {
            if (IsAtEnd())
                return false;

            if (_source[current] != expected)
                return false;

            current++;
            return true;
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object literal)
        {
            string text = _source.Substring(start, current - start);
            _tokens.Add(new Token(type, text, literal, line));
        }

        private char Advance()
        {
            current++;
            return _source[current - 1];
        }
        
        #endregion
    }
}