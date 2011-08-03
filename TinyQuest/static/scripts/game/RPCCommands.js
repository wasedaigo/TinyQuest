function go()
{
  loadJSON("api/Go", function(json)
  {
    if (json["success"])
    {
      loadState("adventure");
    }
  })
}

function charge_energy()
{
  loadJSON("api/ChargeEnergy", function(json)
  {
    if (json["success"])
    {
      loadState("adventure");
    }
  })
}

function getCurrentAdventureScene(cb)
{
  loadJSON("api/GetCurrentAdventureScene", cb);
}