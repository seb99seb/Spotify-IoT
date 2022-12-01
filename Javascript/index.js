Vue.createApp({
    data() {
        return {
            clientId: '849bcded0aa04ffa855b5bd3381c7284',
            clientSecret: '25bed5e8f08a43ac9f914b920dae2b4b',
            scopes: "user-read-playback-state playlist-read-private user-modify-playback-state",
            redirectURI: "http://localhost:5501/",
            deviceId: "",
            auth: "",
            token: "",
            xhr: "",
            xhr2: "",
            settingPlaylist: false,
            logginIn: false,
            tokenDone: false,
            deviceIdDone: false,
            playlistDone: false
        }
    },
    methods: {
        fetchAccessToken(){
            let body = 'grant_type=authorization_code'
            body += '&code=' + this.auth
            body += '&redirect_uri=' + this.redirectURI
            body += '&client_id=' + this.clientId
            body += '&client_secret=' + this.clientSecret
            this.callAuthorizationApi(body)
        },
        callAuthorizationApi(body){
            this.xhr = new XMLHttpRequest()
            this.xhr.open('POST', 'https://accounts.spotify.com/api/token', true)
            this.xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded')
            this.xhr.setRequestHeader('Authorization', 'Basic ' + btoa(this.clientId + ':' + this.clientSecret))
            this.xhr.send(body)
            this.xhr.onload = this.handleAuthorizationResponse
        },
        handleAuthorizationResponse(){
            var data = JSON.parse(this.xhr.responseText)
            this.token = data.access_token
            this.tokenDone = true
            console.log(this.token)
        },
        async getDeviceId(){
            const result = await fetch(`https://api.spotify.com/v1/me/player/devices`, {
                method: 'GET',
                headers: { 'Authorization' : 'Bearer ' + this.token}
            })
            const data = await result.json()
            this.deviceId = data.devices[0].id
            this.deviceIdDone = true
        },
        /*async skip(){
            const result = await fetch(`https://api.spotify.com/v1/me/player/next`, {
                method: 'POST',
                headers: { 'Authorization' : 'Bearer ' + this.token}
            })
        },*/
        /*async playSong(){
            const result = await fetch(`https://api.spotify.com/v1/me/player/play`, {
                method: 'PUT',
                headers: { 'Authorization' : 'Bearer ' + this.token}
            })
        },*/
        playSong(){
            this.xhr2 = new XMLHttpRequest()
            this.xhr2.open('PUT', 'https://api.spotify.com/v1/me/player/play', true)
            this.xhr2.setRequestHeader('Accept', 'application/json')
            this.xhr2.setRequestHeader('Content-Type', 'application/json')
            this.xhr2.setRequestHeader('Authorization', 'Bearer ' + this.token)
            body = 'device_id='+this.deviceId
            this.xhr2.send(body)
            console.log(this.xhr2)
            this.xhr2.onload = this.handlePlayResponse
            console.log(this.xhr2.responseText)
        },
        handlePlayResponse(){
            var data = JSON.parse(this.xhr2.responseText)
            console.log(data)
        },
        async getPlaylists(){
            const result = await fetch(`https://api.spotify.com/v1/me/playlists`, {
                method: 'GET',
                headers: { 'Authorization' : 'Bearer ' + this.token}
            })
            const data = await result.json()
            console.log(data)
            console.log('test')
            this.playlistDone = true
        },
        getFragmentIdentifier(){
            const urlParams = new URLSearchParams(window.location.search);
            this.auth = urlParams.get('code');
            if (urlParams.get('code')!=null){
                this.logginIn=true
            }
        }
    }
}).mount("#app")