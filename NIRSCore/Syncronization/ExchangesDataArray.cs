namespace NIRSCore.Syncronization
{
    public sealed class ExchangesDataArray
    {
        ListExchangesData[] Datas { get; set; }

        public ExchangesDataArray() => Datas = null;

        public ExchangesDataArray(ListExchangesData[] data) => Datas = data;
    }
}
