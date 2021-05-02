using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Kanban.Data;
using Kanban.Dto;
using Kanban.Models;
using Xunit;

namespace Unit.Tests {
    public class UnitTest1 {
        [Fact]
        public void CardTituloTamanhoSucesso () {
            var card = new Card {
                titulo = "aaa",
            };
            Assert.Equal (card.titulo, "aaa");
        }

        [Fact]
        public void LoginRequired () {
            var user = new UserForLoginDto {
                senha = "aaaa",
            };
            Assert.True (ValidateModel (user).Any (
                v => v.MemberNames.Contains ("login") &&
                v.ErrorMessage.Contains ("required")));
        }

        [Fact]
        public void SenhaRequired () {
            var user = new UserForLoginDto {
                login = "aaaa",
            };
            Assert.True (ValidateModel (user).Any (
                v => v.MemberNames.Contains ("senha") &&
                v.ErrorMessage.Contains ("required")));
        }

        private IList<ValidationResult> ValidateModel (object model) {
            var validationResults = new List<ValidationResult> ();
            var ctx = new ValidationContext (model, null, null);
            Validator.TryValidateObject (model, ctx, validationResults, true);
            return validationResults;
        }

    }
}