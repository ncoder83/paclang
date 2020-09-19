﻿using PacLang.CodeAnalysis.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace PacLang.Tests.CodeAnalysis.Syntax
{
    public class SyntaxFactTests
    {
        [Theory]
        [MemberData(nameof(GetSyntaxKindData))]
        public void SyntaxFact_GetText_RondTrips(SyntaxKind kind)
        {

            var text = SyntaxFacts.GetText(kind);

            if (text == null)
                return;

            var tokens = SyntaxTree.ParseTokens(text);
            var token = Assert.Single(tokens);


            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);

        }

        public static IEnumerable<object[]> GetSyntaxKindData()
        {
            var kinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));

            foreach (var kind in kinds)            
                yield return new object[] { kind };            

        }
    }
}
