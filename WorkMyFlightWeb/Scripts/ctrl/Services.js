
module.value("valueService", {
    apiResult: {},
}) 

module.constant("globalConst", {
    app_name: "cards",
    url: `http://localhost:61909/api/AnonymousFacade/GetFutureFlights` 
})

// getting the future flights by ajax to the cards using service
module.service("apiService", CreateApiService)
function CreateApiService($http, globalConst, valueService) {
    this.GetCards = () => {
        $http.get(globalConst.url).then(
            // success
            (resp) => {
                valueService.apiResult.cards = angular.fromJson(resp.data);
                valueService.apiResult.cards.splice(6)
            },
            //error
            (err) => {
                alert('error')
                console.log(err)
            })
    }
}
