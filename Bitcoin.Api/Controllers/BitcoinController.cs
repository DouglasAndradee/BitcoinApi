using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestSharp;
using RestSharp.Serializers.Utf8Json;

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

        public IActionResult TickerBitCoinAsync()
        {

            RestClient client = new RestClient("https://www.mercadobitcoin.net/api/");
            client.UseUtf8Json();

            var request = new RestRequest("BTC/ticker/", DataFormat.Json);

            var response = client.Get(request);

            if (response.StatusCode.ToString() != "OK")
            {
                return BadRequest();
            }

            return Ok(response.Content);
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

        public IActionResult BitCoinTrades()
        {
            RestClient client = new RestClient("https://www.mercadobitcoin.net/api/");
            client.UseUtf8Json();

            var request = new RestRequest("BTC/trades/", DataFormat.Json);

            var response = client.Get(request);

            if (response.StatusCode.ToString() != "OK")
            {
                return BadRequest();
            }

            return Ok(response.Content);
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

            var response = client.Get(request);

            if (response.StatusCode.ToString() != "OK")
            {
                return BadRequest();
            }

            return Ok(response.Content);
        }

    }

}
