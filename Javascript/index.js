const baseUri = "https://sensifyrest2022.azurewebsites.net/api/Moods/playlistId"
Vue.createApp({
    data() {
        return {
            /**Id for logging into our Spotify Dashboard */
            clientId: '849bcded0aa04ffa855b5bd3381c7284',
            /**Essentialy our password for our Spotify Dashboard */
            clientSecret: '25bed5e8f08a43ac9f914b920dae2b4b',
            /**The scopes that we want for our token, allows for use of certain API calls */
            scopes: "user-read-playback-state playlist-read-private user-modify-playback-state",
            /**URI for getting back to our site after going to Spotify's site */
            redirectURI: "http://localhost:5501/",
            /**The id of an open window of spotify, used to play music */
            deviceId: "",
            /**The authentication code needed to get a proper token */
            auth: "",
            /**The token, needed for most Spotify API calls, used in headers */
            token: "",
            /**variable used for doing XMLHTTPRequests */
            xhr: "",
            /**The user id of our specific Spotify account - note: needs Spotify premium to work */
            userId: "31kqpkdzy6346fcuw6jh6tom5a3e",
            /**Bool used to trigger a v-if statement in index.html for whether or not the user
            is setting a playlist to a mood */
            settingPlaylist: false,
            /**Bool used so that only a part of the HTML shows when first entering the site - 
            will be set to true once "auth" has been set */
            logginIn: false,
            /**Bool used for only getting a token once, since every new token overwrites the old one */
            tokenDone: false,
            /**A list of all the playlists the user has on Spotify */
            myPlaylists: [],
            /**The mood that is used for when the user is selecting a mood to add a playlist id to */
            mood: "",
            /**The selected playlist when choosing which playlist to add to a mood */
            PL: "",
            /**Id of the playlist that was selected when confirming to add playlist id to mood */
            playlistId: "",
            /**For seeing if both input fields do not have an option choosen */
            notSelected: false,
            /**For seeing if both input fields have an option choosen */
            selected: false,
            /**The mood given by the user, via Raspberry Pi */
            currentMood: "",
            /**The playlist id that is connected to the string "currentMood", with the default value of
            the playlist id of our happy playlist */
            currentPlaylistId: "2dBlZg79Q5bLYso5yPimy5",
            /**Bool controlling if the user is trying to play their playlist from given mood or not */
            listening: false,
            /**String used to compare to "currentMood", so that it can see whether or not the mood given
            from the Raspberry Pi */
            currentPlayingMood: ""
        }
    },
    methods: {
        /**Method that initializes the process of getting a token, 
        and puts together the body that is sent to Spotify to get it */
        fetchAccessToken(){
            let body = 'grant_type=authorization_code'
            body += '&code=' + this.auth
            body += '&redirect_uri=' + this.redirectURI
            body += '&client_id=' + this.clientId
            body += '&client_secret=' + this.clientSecret
            this.callAuthorizationApi(body)
        },
        /**The Method that sends the POST call to Spotify to retrieve a token */
        callAuthorizationApi(body){
            //defines 'xhr' as an XMLHTTPRequest
            this.xhr = new XMLHttpRequest()
            //setting the call method, the URL it sends to, and true to make async
            this.xhr.open('POST', 'https://accounts.spotify.com/api/token', true)
            //setting headers with necessary information
            this.xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded')
            this.xhr.setRequestHeader('Authorization', 'Basic ' + btoa(this.clientId + ':' + this.clientSecret))
            //sending the XMLHTTPRequest with the body defined earlier
            this.xhr.send(body)
            //sends the data to another method to handle the data
            this.xhr.onload = this.handleAuthorizationResponse
        },
        /**Method that handles the data gotten from the "callAuthorizationApi()" method, the token */
        handleAuthorizationResponse(){
            //converts the data from JSON to data readable by JS
            var data = JSON.parse(this.xhr.responseText)
            //the given data has our access token
            this.token = data.access_token
            //method is called to get the playlists of the user through a button
            this.tokenDone = true
            //console.log(this.token)
        },
        /**Gets the id of one of the open Spotify windows logged on the Spotify account -
        method is async so that "await" can be used, so that the program doesn't just run
        all the code in the method before having fetched the GET call, but instead wait till
        the line is done */
        async getDeviceId(){
            const result = await fetch(`https://api.spotify.com/v1/me/player/devices`, {
                method: 'GET',
                headers: { 'Authorization' : 'Bearer ' + this.token}
            })
            //Handles the data, and saves the first device id gotten from the list of device id's
            const data = await result.json()
            this.deviceId = data.devices[0].id
            //console.log('device id:'+this.deviceId)
        },
        /**Not done */
        async playSong(){
            //Function for adding delay the method, can be used if the while loop loops too fast and
            //makes too many API calls
            function sleep(ms) {
                return new Promise(resolve => setTimeout(resolve, ms));
            }
            //Resets the "currentPlayingMood" value, so that the "playSong()" method works in repeated usage
            this.currentPlayingMood = "temp"
            //While loop that runs while the user is in the listening section of the site
            while(this.listening){
                //Gets the mood that is currently stored in the database, used to check if the current mood
                //has been replaced by the Raspberry Pi
                await this.getCurrentMood()
                console.log('current mood: ' + this.currentMood)
                //If the Raspberry Pi joystick get clicked or points down a "Stop" mood is sent, which
                //pauses the playing song
                if(this.currentMood == "Stop"){
                    this.pauseSong()
                }
                //If the mood gotten from the database is new, this statement goes through
                else if(this.currentMood!=this.currentPlayingMood){
                    //Sets "currentPlayingMood" as "currentMood", so that they can be compared again when
                    //it loops again
                    this.currentPlayingMood = this.currentMood
                    //Makes sure it has the correct device id to use
                    await this.getDeviceId()
                    console.log('device id: ' + this.deviceId)
                    //Gets the new playlist to be played
                    await this.getPlaylistId()
                    console.log('current playlist id: ' + this.currentPlaylistId)
                    //Some randomness based on the length of the given playlist, so that it picks
                    //a random song to play from that playlist
                    var offsetPos = Math.floor(Math.random() * await this.getPlaylistFromId())
                    //Defining the body that is to be send via a PUT call
                    let body = {}
                    //The part of the body that points at what playlist to be used
                    body.context_uri = 'spotify:playlist:' + this.currentPlaylistId
                    body.offset = {}
                    //The part of the body that points at how far into the playlist we start listening to music from
                    body.offset.position = offsetPos
                    this.xhr = new XMLHttpRequest()
                    this.xhr.open('PUT', 'https://api.spotify.com/v1/me/player/play?device_id='+this.deviceId, true)
                    this.xhr.setRequestHeader('Content-Type', 'application/json')
                    this.xhr.setRequestHeader('Authorization', 'Bearer ' + this.token)
                    //Turns the body into JSON, so that it can be understood at Spotify
                    this.xhr.send(JSON.stringify(body))
                }
            }
            //When the while loop stops, the song stops aswell
            this.pauseSong()
            console.log('Stopped listening')
        },
        /**Async method that returns the number of tracks in a playlist via a GET call */
        async getPlaylistFromId(){
            const result = await fetch('https://api.spotify.com/v1/playlists/' + this.currentPlaylistId + '?market=ES', {
                method: 'GET',
                headers: { 'Authorization' : 'Bearer ' + this.token}
            })
            const data = await result.json()
            return data.tracks.total
        },
        /**Pauses the currently playing music via device id */
        async pauseSong(){
            await this.getDeviceId()
            this.xhr = new XMLHttpRequest()
            this.xhr.open('PUT', 'https://api.spotify.com/v1/me/player/pause?device_id='+this.deviceId, true)
            this.xhr.setRequestHeader('Content-Type', 'application/json')
            this.xhr.setRequestHeader('Authorization', 'Bearer ' + this.token)
            this.xhr.send()
        },
        /**Resumes the currently choosen music via device id */
        async resumeSong(){
            await this.getDeviceId()
            this.xhr = new XMLHttpRequest()
            this.xhr.open('PUT', 'https://api.spotify.com/v1/me/player/play?device_id='+this.deviceId, true)
            this.xhr.setRequestHeader('Content-Type', 'application/json')
            this.xhr.setRequestHeader('Authorization', 'Bearer ' + this.token)
            this.xhr.send()
        },
        /**Gets 40 of the playlists registered on a Spotify account */
        getPlaylists(){
            this.xhr = new XMLHttpRequest()
            var url = 'https://api.spotify.com/v1/users/' + this.userId + '/playlists?limit=40'
            this.xhr.open('GET', url, true)
            this.xhr.setRequestHeader('Content-Type', 'application/json')
            this.xhr.setRequestHeader('Authorization', 'Bearer ' + this.token)
            this.xhr.send()
            this.xhr.onload = this.handlePlayslists
        },
        /**Handles the playlists gotten from getPlaylists() */
        handlePlayslists(){
            var data = JSON.parse(this.xhr.responseText)
            this.myPlaylists = data.items
            this.settingPlaylist = true
        },
        /**Resets the variables used in adding a playlist to a mood */
        cancelSettingPlaylist(){
            this.PL = ""
            this.mood = ""
            this.selected = false
            this.notSelected = false
            this.settingPlaylist = false
        },
        /**Used for seeing whether or not both option fields have a value choosen */
        selection(){
            //PL and mood only have a length if an option box has ben chosen
            if (this.PL.length>0 && this.mood.length>0){
                this.selected = true
                this.notSelected = false
            }
            else{
                this.notSelected = true
                this.selected = false
            }
        },
        //Method that is called everytime the volume slider is moved, it sends a PUT call to Spotify 
        //telling them to change their volume level percentage to whatever the volume slider is showing
        async changeVolume(){
            //Gets the value of the volume slider by the id of its element
            var volume = document.getElementById('volume').value
            //Displays the volume percentage besides the volume slider in the HTML
            document.getElementById('showVolume').innerHTML = volume + '%'
            this.xhr = new XMLHttpRequest()
            this.xhr.open('PUT', 'https://api.spotify.com/v1/me/player/volume?device_id='+this.deviceId+'&volume_percent='+volume, true)
            this.xhr.setRequestHeader('Content-Type', 'application/json')
            this.xhr.setRequestHeader('Authorization', 'Bearer ' + this.token)
            this.xhr.send()
        },
        /**Saves the chosen playlist id to the database in accordance to the chosen mood through an API call to our RESTful service */
        async savePlaylistId(){
            this.selected = false
            this.notSelected = false
            this.myPlaylists.forEach(playlist => {
                if (this.PL == playlist.name){
                    this.playlistId = playlist.id
                }
            });
            await axios.put(baseUri + "?mood=" + this.mood + "&playlistId=" + this.playlistId)
        },
        /**A call to our API to retrieve the mood stored in the database */
        async getCurrentMood(){
            await axios.get('https://sensifyrest2022.azurewebsites.net/api/Moods')
            .then(response => (this.currentMood = response.data))
        },
        /**Gets a playlist according to the mood the user has, i.e. the mood chosen on the Raspberry Pi */
        async getPlaylistId(){
            await axios.get(baseUri + "/" + this.currentMood)
            .then(response => (this.currentPlaylistId = response.data))
            //console.log(this.currentPlaylistId)
        },
        /**Reads the information in the URL, used for when Spotify redirects back to the site with our authorization code */
        getFragmentIdentifier(){
            //Searches the URL and sets "urlParams" as it
            const urlParams = new URLSearchParams(window.location.search);
            //checks if the string 'code' occurs in the URL, if it's there 'auth' will be set as the string that is behind
            this.auth = urlParams.get('code');
            if (urlParams.get('code')!=null){
                //sets logginIn as true, so that the rest of the website is shown and the first part hidden
                this.logginIn=true
            }
        }
    }
}).mount("#app")