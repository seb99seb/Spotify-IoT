const baseUri = "http://localhost:5093/api/Moods/playlistId"
Vue.createApp({
    data() {
        return {
            /**Id for logging into our Spotify Dashboard */
            clientId: '849bcded0aa04ffa855b5bd3381c7284',
            /**Essentialy our password for our Spotify Dashboard */
            clientSecret: '25bed5e8f08a43ac9f914b920dae2b4b',
            /**The scopes that we want for our token, allows for use of certain API calls */
            scopes: "user-read-playback-state playlist-read-private user-modify-playback-state",
            /**URI for getting back to our site af going to Spotify's site */
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
            currentMood: "Happy",
            /**The playlist id that is connected to the string "currentMood" */
            currentPlaylistId: ""
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
            document.getElementById("getPL").click()
            this.tokenDone = true
            //console.log(this.token)
        },
        /**Gets the id of one of the open Spotify windows logged on the Spotify account */
        async getDeviceId(){
            const result = await fetch(`https://api.spotify.com/v1/me/player/devices`, {
                method: 'GET',
                headers: { 'Authorization' : 'Bearer ' + this.token}
            })
            const data = await result.json()
            this.deviceId = data.devices[0].id
            //console.log('device id:'+this.deviceId)
        },
        /**Not done */
        async playSong(){
            await this.getDeviceId()
            this.xhr = new XMLHttpRequest()
            this.xhr.open('PUT', 'https://api.spotify.com/v1/me/player/play?device_id='+this.deviceId, true)
            this.xhr.setRequestHeader('Content-Type', 'application/json')
            this.xhr.setRequestHeader('Authorization', 'Bearer ' + this.token)
            this.xhr.send()
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
        /**Saves the chosen playlist id to the database in accordance to the chosen mood through an API call to our RESTful service */
        async savePlaylistId(){
            this.myPlaylists.forEach(playlist => {
                if (this.PL == playlist.name){
                    this.playlistId = playlist.id
                }
            });
            await axios.put(baseUri + "?mood=" + this.mood + "&playlistId=" + this.playlistId)
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