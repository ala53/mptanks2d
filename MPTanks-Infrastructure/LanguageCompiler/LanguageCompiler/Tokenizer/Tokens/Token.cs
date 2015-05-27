using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageCompiler.Tokenizer.Tokens
{
    class Token
    {
        public TokenType Type { get; set; }
        public object Value { get; set; }
    }
}
