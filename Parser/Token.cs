namespace PSI;
using static Token.E;

// Represents a PSI language Token
public class Token {
   public Token (Tokenizer source, E kind, string text, int line, int column, string msg)
      => (Source, Kind, Text, Line, Column, ErrorMessage) = (source, kind, text, line, column, ErrorMessage + msg);
   public Tokenizer Source { get; }
   public E Kind { get; }
   public string Text { get; }
   public int Line { get; }
   public int Column { get; }
   public string ErrorMessage { get; } = "";

   // The various types of token
   public enum E {
      // Keywords
      PROGRAM, VAR, IF, THEN, WHILE, ELSE, FOR, TO, DOWNTO,
      DO, BEGIN, END, PRINT, TYPE, NOT, OR, AND, MOD, _ENDKEYWORDS,
      // Operators
      ADD, SUB, MUL, DIV, NEQ, LEQ, GEQ, EQ, LT, GT, ASSIGN,
      _ENDOPERATORS,
      // Punctuation
      SEMI, PERIOD, COMMA, OPEN, CLOSE, COLON,
      _ENDPUNCTUATION,
      // Others
      IDENT, INTEGER, REAL, BOOLEAN, STRING, CHAR, EOF, ERROR
   }

   // Print a Token
   public override string ToString () => Kind switch {
      EOF or ERROR => Text,
      < _ENDKEYWORDS => $"\u00ab{Kind.ToString ().ToLower ()}\u00bb",
      STRING => $"\"{Text}\"",
      CHAR => $"'{Text}'",
      _ => Text,
   };

   // Utility function used to echo an error to the console
   public void PrintError () {
      if (Kind != ERROR) throw new Exception ("PrintError called on a non-error token");
      int start = Line > 3 ? Line - 3 : 0, end = Line + 2 <= Source.Lines.Length ? Line + 2 : Source.Lines.Length, line = start + 1;
      Console.WriteLine ($"File: {Source.FileName}\n───┬────────────────");
      foreach (var str in Source.Lines[start..Line]) Console.WriteLine ($"{line++,3}│ {str}");
      mX = Source.Lines[Line - 1].IndexOf ($"{Text}");
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine ("^", Console.CursorLeft = mX + 5);
      Console.WriteLine ($"{ErrorMessage}", Console.CursorLeft = Math.Max (mX, Column - ErrorMessage.Length / 2) + 1);
      Console.ResetColor ();
      Console.WriteLine ($"{line++,3}│ {string.Join ($"\n{line++,3}│ ", Source.Lines[Line..end])}");
   }
   static int mX;

   // Helper used by the parser (maps operator sequences to E values)
   public static List<(E Kind, string Text)> Match = new () {
      (NEQ, "<>"), (LEQ, "<="), (GEQ, ">="), (ASSIGN, ":="), (ADD, "+"),
      (SUB, "-"), (MUL, "*"), (DIV, "/"), (EQ, "="), (LT, "<"),
      (LEQ, "<="), (GT, ">"), (SEMI, ";"), (PERIOD, "."), (COMMA, ","),
      (OPEN, "("), (CLOSE, ")"), (COLON, ":")
   };
}