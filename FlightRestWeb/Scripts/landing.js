setInterval(function () {
    window.location.reload(1);
}, 300000);

let flights = new Array();
let jsonFlights = '';
let flightsStorage = '';
let statusColor = '';
let status = '';
let sqlDateStr = ''; // as for MySQL DATETIME
function LandingeStatus(LandingTime) {

    timeNow = new Date();

    if (timeNow > LandingTime) {
        status = 'LANDED'
    }
    else if (timeNow.getHours() - LandingTime.getHours() == 0 && timeNow.getMinutes() - LandingTime.getMinutes() < 15) {
        status = 'LANDING'
    }
    else if (LandingTime.getHours() - timeNow.getHours() <= 2 && LandingTime.getHours() - timeNow.getHours() > 0) {
        status = 'FINAL'
    }
    else {
        let rangePerc = Math.floor(Math.random() * 10 + 1);
        if (rangePerc <= 2) {
            return status = 'DELAYED';
        }
        status = 'NOT FINAL'
    }
    return status;
}
function LandingStatusColor(status) {

    if (status == 'LANDED') {
        statusColor = 'greenDepart';
    }
    else if (status == 'LANDING') {
        statusColor = 'orangeLanding';
    }
    else if (status == 'FINAL') {
        statusColor = 'finalNotFinalLanding';
    }
    else if (status == 'NOT FINAL') {
        statusColor = 'finalNotFinalLanding';
    }
    else if (status == 'DELAYED') {
        statusColor = 'redDepart';
    }
    return statusColor;
}
function AdujstTime(LandingTime) {
        // compare my sql date to java script date
    sqlDateStr = LandingTime.toLocaleString().replace('T', ' '); // as for MySQL 
    sqlDateStr = sqlDateStr.replace(/:| /g, "-");
    let YMDhms = sqlDateStr.split("-");
    jsDate = new Date();
    jsDate.setFullYear(parseInt(YMDhms[0]), parseInt(YMDhms[1]) - 1,
        parseInt(YMDhms[2]));
    jsDate.setHours(parseInt(YMDhms[3]), parseInt(YMDhms[4]),
        parseInt(YMDhms[5]), 0/*msValue*/);
    return jsDate;
}
function StatusDelay(delayDate) {

        let randomMin = Math.floor((Math.random() * 10 + 1) * 6);
        let randomHours = Math.floor(Math.random() * 10 + 1);
        while (randomMin <= 30 && randomMin > 60) {
            randomMin = Math.floor((Math.random() * 10 + 1) * 6);
        }
        while (randomHours >= 3) {
            randomHours = Math.floor(Math.random() * 10 + 1);
        }
    delayDate.setMinutes(jsDate.getMinutes() + randomMin);
    delayDate.setHours(jsDate.getHours() + randomHours);
    return delayDate;

}
if (navigator.onLine ) {

    let lastOnLine = new Date();
    lastOnLine.setHours(lastOnLine.getHours() + 3);
    lastOnLine = lastOnLine.toUTCString().replace('GMT', ' ');
    jsonDate = JSON.stringify(lastOnLine);
    localStorage.setItem('lastOnLineLanding', jsonDate);
    $(document).ready(() => {
        $tableFlights = $("#flightsTable")
        $.ajax({
            url: "/api/AnonymousFacade/GetArrivingNow"
        })
            .then((data) => {

                console.log(data);
                $tableFlights.append(`
                        <tr>
                        <th>AIR LINE NAME</th>
                        <th>FLIGHT ID</th>
                        <th>ORIGIN COUNTRY NAME</th>
                        <th>DESTINATION COUNTRY NAME</th>
                        <th>LandingTime</th>
                        <th>Status</th>
                        </tr>`)
                $.each(data, (i, oneFlight) => {

                    oneFlight.LandingTime = oneFlight.LandingTime.toLocaleString().replace('T', ' '); /* DATETIME without T*/
                    oneFlight.LandingTime = AdujstTime(oneFlight.LandingTime);
                    oneFlight.status = LandingeStatus(oneFlight.LandingTime);
                    if (oneFlight.status == 'DELAYED') {
                        oneFlight.LandingTime = StatusDelay(oneFlight.LandingTime);

                    }
                    statusColor = LandingStatusColor(oneFlight.status);
                    oneFlight.LandingTime.setHours(oneFlight.LandingTime.getHours() + 3);
                    oneFlight.LandingTime = oneFlight.LandingTime.toUTCString().replace('GMT', ' ');
                    flights.push(oneFlight);

                    $tableFlights.append(
                        `<tr><td> ${oneFlight.AirlineName} </td>
                             <td>${oneFlight.ID}</td>
                             <td>${oneFlight.OriginCountryName}</td>
                             <td>${oneFlight.DestinationCountryName}</td>
                             <td>${oneFlight.LandingTime}</td>
                             <td class="${statusColor}">${oneFlight.status}</td ></tr >`)

                })
                jsonFlights = JSON.stringify(flights);
                localStorage.setItem('landingFlights', jsonFlights);

            })
            .catch((err) => { console.log(err) })
    })
}
else { reload();}
function reload()
    {
    let lastDate = JSON.parse(localStorage['lastOnLineLanding']);
    swal.fire('No Connection', 'LastOnLine : ' + lastDate); 
        flightsStorage = JSON.parse(localStorage['landingFlights']);
        console.log(flightsStorage);
    $tableFlights = $("#flightsTable");
        $tableFlights.append(`
                        <tr>
                        <th>AIR LINE NAME</th>
                        <th>FLIGHT ID</th>
                        <th>ORIGIN COUNTRY NAME</th>
                        <th>DESTINATION COUNTRY NAME</th>
                        <th>LandingTime</th>
                        <th>Status</th>
                        </tr>`)

        $.each(flightsStorage, (i, oneFlight) => {

            statusColor = LandingStatusColor(oneFlight.status);

            $tableFlights.append(
                `<tr><td> ${oneFlight.AirlineName} </td>
                             <td>${oneFlight.ID}</td>
                             <td>${oneFlight.OriginCountryName}</td>
                             <td>${oneFlight.DestinationCountryName}</td>
                             <td>${oneFlight.LandingTime}</td>
                             <td class="${statusColor}">${oneFlight.status}</td></tr>`)

        })

}

            



