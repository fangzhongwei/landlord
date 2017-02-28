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

            cardTagDic.Add(103,"card_c3");
            cardTagDic.Add(104,"card_c4");
            cardTagDic.Add(105,"card_c5");
            cardTagDic.Add(106,"card_c6");
            cardTagDic.Add(107,"card_c7");
            cardTagDic.Add(108,"card_c8");
            cardTagDic.Add(109,"card_c9");
            cardTagDic.Add(110,"card_c10");
            cardTagDic.Add(111,"card_cj");
            cardTagDic.Add(112,"card_cq");
            cardTagDic.Add(113,"card_ck");
            cardTagDic.Add(114,"card_cq");
            cardTagDic.Add(115,"card_c2");

            cardTagDic.Add(203,"card_d3");
            cardTagDic.Add(204,"card_d4");
            cardTagDic.Add(205,"card_d5");
            cardTagDic.Add(206,"card_d6");
            cardTagDic.Add(207,"card_d7");
            cardTagDic.Add(208,"card_d8");
            cardTagDic.Add(209,"card_d9");
            cardTagDic.Add(210,"card_d10");
            cardTagDic.Add(211,"card_dj");
            cardTagDic.Add(212,"card_dq");
            cardTagDic.Add(213,"card_dk");
            cardTagDic.Add(214,"card_da");
            cardTagDic.Add(215,"card_d2");

            cardTagDic.Add(303,"card_h3");
            cardTagDic.Add(304,"card_h4");
            cardTagDic.Add(305,"card_h5");
            cardTagDic.Add(306,"card_h6");
            cardTagDic.Add(307,"card_h7");
            cardTagDic.Add(308,"card_h8");
            cardTagDic.Add(309,"card_h9");
            cardTagDic.Add(310,"card_h10");
            cardTagDic.Add(311,"card_hj");
            cardTagDic.Add(312,"card_hq");
            cardTagDic.Add(313,"card_hk");
            cardTagDic.Add(314,"card_ha");
            cardTagDic.Add(315,"card_h2");

            cardTagDic.Add(403,"card_s3");
            cardTagDic.Add(404,"card_s4");
            cardTagDic.Add(405,"card_s5");
            cardTagDic.Add(406,"card_s6");
            cardTagDic.Add(407,"card_s7");
            cardTagDic.Add(408,"card_s8");
            cardTagDic.Add(409,"card_s9");
            cardTagDic.Add(410,"card_s10");
            cardTagDic.Add(411,"card_sj");
            cardTagDic.Add(412,"card_sq");
            cardTagDic.Add(413,"card_sk");
            cardTagDic.Add(414,"card_sa");
            cardTagDic.Add(415,"card_s2");

            cardTagDic.Add(516,"card_jb");
            cardTagDic.Add(517,"card_jr");
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