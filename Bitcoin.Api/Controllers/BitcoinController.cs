using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestSharp;
using RestSharp.Serializers.Utf8Json;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Bitcoin.Api.Models;
using Newtonsoft.Json;

namespace Bitcoin.Api.Controllers
{
    [Route("v1/bitcoin")]
    [ApiController]
    public class BitcoinController : ControllerBase
    {
        /// <summary>
        /// Retorna informações com o resumo das últimas 24 horas de negociações(necessita de autenticação).
        /// </summary>
        /// <response code = "200">
        /// Seus parâmetros são:<br />
        /// high: Maior preço unitário de negociação das últimas 24 horas.<br />
        /// low: Menor preço unitário de negociação das últimas 24 horas.<br />
        /// vol: Quantidade negociada nas últimas 24 horas.<br />
        /// last: Preço unitário da última negociação.<br />
        /// buy: Maior preço de oferta de compra das últimas 24 horas.<br />
        /// sell: Menor preço de oferta de venda das últimas 24 horas.<br />
        /// date: Data e hora da informação em Era Unix.<br />
        /// </response>

        [HttpGet]
        [Route("ticker")]
        [Authorize(Roles = "ADMIN")]

        public IActionResult TickerBitCoinAsync([FromHeader(Name = "Authorization")][Required] string requiredHeader)
        {

            RestClient client = new RestClient("https://www.mercadobitcoin.net/api/");
            client.UseUtf8Json();

            var request = new RestRequest("BTC/ticker/", DataFormat.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = client.Get(request);

            if (response.StatusCode.ToString() != "OK")
            {
                return BadRequest();
            }
            var tickers = JsonConvert.DeserializeObject<Result>(response.Content);

            return Ok(tickers);
        }

        /// <summary>
        /// Retorna um array do histórico de negociações realizadas(necessita de autenticação).
        /// </summary>
        /// <response code = "200">
        /// Seus parâmetros são:<br />
        /// date: Data e hora da negociação em era Unix\p.<br />
        /// price: Preço unitário da negociação.<br />
        /// amount: Quantidade da negociação.<br />
        /// tid: Identificador da negociação.<br />
        /// type: Indica a ponta executora da negociação.<br />
        /// </response>

        [HttpGet]
        [Route("trades")]
        [Authorize(Roles = "ADMIN,MANAGER")]

        public IActionResult BitCoinTrades([FromHeader(Name = "Authorization")][Required] string requiredHeader)
        {

            RestClient client = new RestClient("https://www.mercadobitcoin.net/api/");
            client.UseUtf8Json();

            var request = new RestRequest("BTC/trades/", DataFormat.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = client.Get(request);

            if (response.StatusCode.ToString() != "OK")
            {
                return BadRequest();
            }
            var trades = JsonConvert.DeserializeObject<List<Trade>>(response.Content);

            return Ok(trades);
        }

        /// <summary>
        /// Retorna um livro de ofertas.
        /// </summary>
        /// <response code = "200">
        /// Livro de ofertas é composto por duas listas: (1) uma lista com as ofertas de compras
        /// ordenadas pelo maior valor; (2) uma lista com as ofertas de venda ordenadas pelo menor valor.
        /// O livro mostra até 1000 ofertas de compra e até 1000 ofertas de venda.
        /// </response>

        [HttpGet]
        [Route("orderbook")]
        [AllowAnonymous]
        public IActionResult BitCoinOrderBook()
        {
            RestClient client = new RestClient("https://www.mercadobitcoin.net/api");
            client.UseUtf8Json();

            var request = new RestRequest("/BTC/orderbook", DataFormat.Json);

            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = client.Get(request);

            if (response.StatusCode.ToString() != "OK")
            {
                return BadRequest();
            }
            var orderBook = JsonConvert.DeserializeObject<OrderBook>(response.Content);

            return Ok(orderBook);
        }

    }

}
