namespace NIRSCore.Syncronization
{
    public sealed class ExchangesDataArray
    {
        public ListExchangesData[] Datas { get; set; }

        public ExchangesDataArray() => Datas = null;

        public ExchangesDataArray(ListExchangesData[] data) => Datas = data;
    }
}
