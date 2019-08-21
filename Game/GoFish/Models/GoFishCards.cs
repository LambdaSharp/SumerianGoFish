using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.GoFish.Models {

    public static class GoFishCards {
        public static List<Card> Init(string uriToCardImage = "") {
            return new List<Card> {
                new Card { Id = "1", Name = "amaretto sour", CardImage = $"{uriToCardImage}/amaretto_sour.png"},
                new Card { Id = "2", Name = "amaretto sour", CardImage = $"{uriToCardImage}/amaretto_sour.png"},
                new Card { Id = "3", Name = "bloody mary", CardImage = $"{uriToCardImage}/bloody_mary.png"},
                new Card { Id = "4", Name = "bloody mary", CardImage = $"{uriToCardImage}/bloody_mary.png"},
                new Card { Id = "5", Name = "cosmopolitan", CardImage = $"{uriToCardImage}/cosmopolitan.png"},
                new Card { Id = "6", Name = "cosmopolitan", CardImage = $"{uriToCardImage}/cosmopolitan.png"},
                new Card { Id = "7", Name = "long island ice tea", CardImage = $"{uriToCardImage}/long_island_ice_tea.png"},
                new Card { Id = "8", Name = "long island ice tea", CardImage = $"{uriToCardImage}/long_island_ice_tea.png"},
                new Card { Id = "9", Name = "mai tai", CardImage = $"{uriToCardImage}/mai_tai.png"},
                new Card { Id = "10", Name = "mai tai", CardImage = $"{uriToCardImage}/mai_tai.png"},
                new Card { Id = "11", Name = "margarita", CardImage = $"{uriToCardImage}/margarita.png"},
                new Card { Id = "12", Name = "margarita", CardImage = $"{uriToCardImage}/margarita.png"},
                new Card { Id = "13", Name = "martini", CardImage = $"{uriToCardImage}/martini.png"},
                new Card { Id = "14", Name = "martini", CardImage = $"{uriToCardImage}/martini.png"},
                new Card { Id = "15", Name = "mojito", CardImage = $"{uriToCardImage}/mojito.png"},
                new Card { Id = "16", Name = "mojito", CardImage = $"{uriToCardImage}/mojito.png"},
                new Card { Id = "17", Name = "moscow mule", CardImage = $"{uriToCardImage}/moscow_mule.png"},
                new Card { Id = "18", Name = "moscow mule", CardImage = $"{uriToCardImage}/moscow_mule.png"},
                new Card { Id = "19", Name = "old fashioned", CardImage = $"{uriToCardImage}/old_fashioned.png"},
                new Card { Id = "20", Name = "old fashioned", CardImage = $"{uriToCardImage}/old_fashioned.png"},
                new Card { Id = "21", Name = "pina colada", CardImage = $"{uriToCardImage}/pina_colada.png"},
                new Card { Id = "22", Name = "pina colada", CardImage = $"{uriToCardImage}/pina_colada.png"},
                new Card { Id = "23", Name = "whiskey sour", CardImage = $"{uriToCardImage}/whiskey_sour.png"},
                new Card { Id = "24", Name = "whiskey sour", CardImage = $"{uriToCardImage}/whiskey_sour.png"},
                new Card { Id = "25", Name = "white russian", CardImage = $"{uriToCardImage}/white_russian.png"},
                new Card { Id = "26", Name = "white russian", CardImage = $"{uriToCardImage}/white_russian.png"}
            }.ShuffleDeck().ToList();
        }
        
        public static List<T> ShuffleDeck<T>(this List<T> list) {  
            var random = new Random();
            for(var i = list.Count - 1; i > 1; i--) {
                var rnd = random.Next(i + 1);  

                T value = list[rnd];  
                list[rnd] = list[i];  
                list[i] = value;
            }
            return list;
        }
    }
}