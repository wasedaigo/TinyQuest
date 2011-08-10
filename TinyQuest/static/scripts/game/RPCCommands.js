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

function getCurrentActiveScene(cb)
{
  loadJSON("api/GetCurrentActiveScene", cb);
}