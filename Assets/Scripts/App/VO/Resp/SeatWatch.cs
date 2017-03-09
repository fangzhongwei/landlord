namespace App.VO
{
    public class SeatWatch
    {
        public long gameId { get; set; }
        public long seatId { get; set; }
        public int gameType { get; set; }
        public int deviceType { get; set; }
        public string cards { get; set; }
        public string landlordCards { get; set; }
        public string proCardsInfo { get; set; }
        public int  baseAmount { get; set; }
        public int  multiples { get; set; }
        public string previousNickname { get; set; }
        public int previousCardsCount { get; set; }
        public string nextNickname { get; set; }
        public int nextCardsCount { get; set; }
        public string playStatus { get; set; }
        public bool landlord { get; set; }
        public string fingerPrint { get; set; }
        public int seqInGame { get; set; }
    }
}