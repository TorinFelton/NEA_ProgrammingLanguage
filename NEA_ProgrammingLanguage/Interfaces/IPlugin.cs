using DataStructures;
using Lexer_Module;
using Parser_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface IPlugin
    {
        string Name { get; }

    }

    public interface IParseHandler
    {
        Step Result(ref TokenQueue tokQ, Token currentTok);
    }
}
