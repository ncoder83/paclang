namespace PacLang.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts
    {
        /*
           example
            -1 * 3

           tree structure (one way)
                
                    -
                    |
                    *
                   / \
                  1   3
                      
            tree structure (another way)                    
                    
                    *
                   / \
                  -   3
                  |
                  1
         
         */
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {                
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 3;
                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 2;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 1;
                default:
                    return 0;
            }
        }


        
    }
}
