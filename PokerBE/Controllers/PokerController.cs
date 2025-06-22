using Microsoft.AspNetCore.Mvc;
using PokerBE.Classes;
using PokerBE.DTOs;
using PokerBE.Services;

namespace PokerBE.Api.Controllers
{
  [ApiController]
  [Route("api/play")]
  public class PokerController : ControllerBase
  {
    /*
      POST api/poker/play
      Accepts a JSON array of player names,
      starts a new FiveCardStud game,
      and returns the game result.
    */
    [HttpPost("fiveCardStud")]
    public ActionResult<GameResultDto> Play([FromBody] List<string> playerNames)
    {
      if (playerNames == null || playerNames.Count == 0)
        return BadRequest("At least one player name must be provided.");

      try
      {
        var game = new FiveCardStud(playerNames);
        var result = game.Deal();

        // Map internal DealResult to DTO
        var dto = new GameResultDto
        {
          PlayerResults = result.Hands.Select(ph => new PlayerResultDto
          {
            Player = ph.Player,
            Cards = ph.Cards.Select(c => c.Display).ToList(),
            Rank = ph.Rank.RankName,
            HandSummary = GameSummary.GetHandSummary(ph.Rank)
          }).ToList(),
          Winners = result.Winners,
          // Use new helper for a friendly winner summary string
          Reason = GameSummary.GetWinnerSummary(result.Winners, result.Hands.FirstOrDefault(ph => ph.Player == result.Winners.First())?.Rank)
        };

        return Ok(dto);
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
      }
    }
  }
}
