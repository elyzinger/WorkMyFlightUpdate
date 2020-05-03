//swal.fire('Search Page')
function onSearchClick() {
    const data = {
        ID: null,
        AirlineName: null,
        DesCountry: null,
        OriCountry: null,
        FlightType: null,
        Selected: null,
    };
    var landChack = $("#landingCheck").prop("checked");
    var departChack = $("#departCheck").prop("checked");
    if (landChack == true && departChack == false) {
        data.FlightType = 'Landing'
    }
    else if (landChack == false && departChack == true) {
        data.FlightType = 'Departure'
    }
    else {
        data.FlightType = 'All'
    }
    const optionType = $("#options").val();
    const searchType = $("#srchT").val();
    switch (optionType) {
        case "flightNum":
            {
                data.ID = searchType;
                break;
            }
        case "airlineName":
            {
                data.AirlineName = searchType;
                break;
            }
        case "destinationCountry":
            {
                data.DesCountry = searchType;
                break;
            }
        case "originCountry":
            {
                data.OriCountry = searchType;
                break;
            }
        case "Selected":
            {
                data.Selected = searchType;
            }
         
    }
    $("#flightTableSearch").empty();
   
    {
        $.ajax({
            url: "/api/AnonymousFacade/SearchByParams",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data)
        }).then((result) => {
            $("#flightTableSearch").append(`
        <tr>
        <th>AIR LINE NAME</th>
        <th>FLIGHT ID</th>
        <th>ORIGIN COUNTRY NAME</th>
        <th>DESTINATION COUNTRY NAME</th>
        <th>DEPARTING TIME</th>
        <th>LANDING TIME</th>
       
                        </tr>`)
            $.each(result, (i, searchFlight) => {
                $("#flightTableSearch").append("<tr>" +
                    "<td>" + searchFlight.AirlineName + "</td>" +
                    "<td>" + searchFlight.ID + "</td>" +
                    "<td>" + searchFlight.OriginCountryName + "</td>" +
                    "<td>" + searchFlight.DestinationCountryName + "</td>" +
                    "<td>" + searchFlight.DepartureTime + "</td>" +
                    "<td>" + searchFlight.LandingTime + "</td>" +
                    "</tr>")
            });

            })
        .catch ((err) => { console.log(err) })
    }

}

