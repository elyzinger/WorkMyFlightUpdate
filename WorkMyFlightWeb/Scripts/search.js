// take all the information from the search page and sends an api request
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
   // sending an api request with all the info from the search func then we get back the table back from sql 
    {
        $.ajax({
            url: "/api/AnonymousFacade/SearchByParams",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data)
        }).then((result) => {
            $("#flightTableSearch").append(`
        <tr>
        <th></th>
        <th>AIRLINE NAME</th>
        <th>FLIGHT ID</th>
        <th>ORIGIN COUNTRY NAME</th>
        <th>DESTINATION COUNTRY NAME</th>
        <th>DEPARTING TIME</th>
        <th>LANDING TIME</th>
       
                        </tr>`)
            $.each(result, (i, searchFlight) => {
                $("#flightTableSearch").append(
                    `<tr> +
                    <td><img class="companyimage" src="https://logo.clearbit.com/${searchFlight.AirlineName}.com" alt="Logo" width="35" height="35"></td>
                    <td> ${searchFlight.AirlineName} </td>
                    <td>${searchFlight.ID}</td>
                    <td>${searchFlight.OriginCountryName}</td>
                    <td>${searchFlight.DestinationCountryName}</td>
                    <td>${searchFlight.DepartureTime}</td>
                    <td>${searchFlight.LandingTime}</td>
                    </tr>`)
            });

            })
        .catch ((err) => { console.log(err) })
    }

}

