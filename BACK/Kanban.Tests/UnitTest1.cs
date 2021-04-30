using System;
using Xunit;
using Kanban.Models;

namespace Unit.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CardTituloTamanhoSucesso()
        {
            var card = new Card {
                titulo = "aaa",
            };
            Assert.Equal(card.titulo, "aaa");
        }

        [Fact]
        public void CardTituloTamanhoFalha()
        {
            var card = new Card {
                titulo = "aa",
            };
            Assert.Equal(card.titulo, "aaa");
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
