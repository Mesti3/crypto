﻿using Binance.Net.Clients;
using Binance.Net.Enums;
using Crypto.Model;
using Crypto.Model.Entities;

namespace Crypto.BinanceControllers
{
    internal class TestBinanceController : IBinanceController
    {
        private ModelConverter Converter;
        public TestBinanceController(ModelConverter convertor)
        {
            Converter = convertor;
        }

        public async Task<Purchase> Buy(string symbol, decimal quantity, decimal price)
        {
            using (var client = new BinanceClient())
            {
                var result = await client.SpotApi.Trading.PlaceTestOrderAsync(symbol, OrderSide.Buy, SpotOrderType.Limit, quantity: quantity, price: price, timeInForce: TimeInForce.GoodTillCanceled);//, newClientOrderId: ClientOrderId.GetNew(symbol)
                if (result.Success)
                    return Converter.BinancePlacedOrderToPurchase(result.Data);
                else
                    throw new Exception("BUY_TEST: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\", \"price\"" + price + "\"}", new Exception(result.Error?.Message));
            }
        }
        public async Task<Purchase> Buy(string symbol, decimal quantity)
        {
            using (var client = new BinanceClient())
            {
                var result = await client.SpotApi.Trading.PlaceTestOrderAsync(symbol, OrderSide.Buy, SpotOrderType.Market, quantity: quantity);//, newClientOrderId: ClientOrderId.GetNew(symbol)
                if (result.Success)
                    return Converter.BinancePlacedOrderToPurchase(result.Data);
                else
                    throw new Exception("BUY_TEST: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\"}", new Exception(result.Error?.Message));
            }
        }
    }
}


