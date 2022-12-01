Vue.createApp({
    data() {
        return {
            clientId: '849bcded0aa04ffa855b5bd3381c7284',
            clientSecret: '25bed5e8f08a43ac9f914b920dae2b4b',
            scopes: "user-read-playback-state playlist-read-private user-modify-playback-state",
            redirectURI: "http://localhost:5501/",
            startupDone: false,
            deviceId: "",
            auth: "",
            token: "",
            xhr: ""
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
            console.log(this.xhr)
            this.xhr.onload = this.handleAuthorizationResponse
            console.log(this.xhr.responseText)
        },
        handleAuthorizationResponse(){
            var data = JSON.parse(this.xhr.responseText)
            this.token = data.access_token
            console.log(this.token)
        },
        async getDeviceId(){
            const result = await fetch(`https://api.spotify.com/v1/me/player/devices`, {
                method: 'GET',
                headers: { 'Authorization' : 'Bearer ' + this.token}
            })
            const data = await result.json()
            this.deviceId = data.devices[0].id
            console.log(this.deviceId)
        },
        getFragmentIdentifier(){
            const urlParams = new URLSearchParams(window.location.search);
            this.auth = urlParams.get('code');
        }
    }
}).mount("#app")