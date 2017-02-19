using System.Collections.Generic;

namespace Assets.Scripts.App.Helper
{
    public class CardHelper
    {
        private static readonly CardHelper instance = new CardHelper();

        private readonly Dictionary<int, string> cardTagDic;

        private CardHelper()
        {
            cardTagDic = new Dictionary<int, string>();

            cardTagDic.Add(103,"Club_3");
            cardTagDic.Add(104,"Club_4");
            cardTagDic.Add(105,"Club_5");
            cardTagDic.Add(106,"Club_6");
            cardTagDic.Add(107,"Club_7");
            cardTagDic.Add(108,"Club_8");
            cardTagDic.Add(109,"Club_9");
            cardTagDic.Add(110,"Club_10");
            cardTagDic.Add(111,"Club_J");
            cardTagDic.Add(112,"Club_Q");
            cardTagDic.Add(113,"Club_K");
            cardTagDic.Add(114,"Club_A");
            cardTagDic.Add(115,"Club_2");

            cardTagDic.Add(203,"Diamond_3");
            cardTagDic.Add(204,"Diamond_4");
            cardTagDic.Add(205,"Diamond_5");
            cardTagDic.Add(206,"Diamond_6");
            cardTagDic.Add(207,"Diamond_7");
            cardTagDic.Add(208,"Diamond_8");
            cardTagDic.Add(209,"Diamond_9");
            cardTagDic.Add(210,"Diamond_10");
            cardTagDic.Add(211,"Diamond_J");
            cardTagDic.Add(212,"Diamond_Q");
            cardTagDic.Add(213,"Diamond_K");
            cardTagDic.Add(214,"Diamond_A");
            cardTagDic.Add(215,"Diamond_2");

            cardTagDic.Add(303,"Heart_3");
            cardTagDic.Add(304,"Heart_4");
            cardTagDic.Add(305,"Heart_5");
            cardTagDic.Add(306,"Heart_6");
            cardTagDic.Add(307,"Heart_7");
            cardTagDic.Add(308,"Heart_8");
            cardTagDic.Add(309,"Heart_9");
            cardTagDic.Add(310,"Heart_10");
            cardTagDic.Add(311,"Heart_J");
            cardTagDic.Add(312,"Heart_Q");
            cardTagDic.Add(313,"Heart_K");
            cardTagDic.Add(314,"Heart_A");
            cardTagDic.Add(315,"Heart_2");

            cardTagDic.Add(403,"Spade_3");
            cardTagDic.Add(404,"Spade_4");
            cardTagDic.Add(405,"Spade_5");
            cardTagDic.Add(406,"Spade_6");
            cardTagDic.Add(407,"Spade_7");
            cardTagDic.Add(408,"Spade_8");
            cardTagDic.Add(409,"Spade_9");
            cardTagDic.Add(410,"Spade_10");
            cardTagDic.Add(411,"Spade_J");
            cardTagDic.Add(412,"Spade_Q");
            cardTagDic.Add(413,"Spade_K");
            cardTagDic.Add(414,"Spade_A");
            cardTagDic.Add(415,"Spade_2");

            cardTagDic.Add(516,"BlackJoker");
            cardTagDic.Add(517,"RedJoker");
        }

        public static CardHelper GetInstance()
        {
            return instance;
        }

        public string GetTag(int cardId)
        {
            return cardTagDic[cardId];
        }
    }
}