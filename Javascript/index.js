Vue.createApp({
    data() {
        return {
            clientId: '849bcded0aa04ffa855b5bd3381c7284',
            clientSecret: '25bed5e8f08a43ac9f914b920dae2b4b',
            token: "",
            scopes: "user-read-playback-state",
            redirectURI: "http://localhost:5501/",
            startupDone: false,
            getCategoriesDone: false,
            genres: [],
            deviceId: "",
            deviceIdGotten: false,
            goodtoken: ""
        }
    },
    methods: {
        async getToken(){
            const result = await fetch('https://accounts.spotify.com/api/token', {
                method: 'POST',
                headers: {
                    'Content-Type' : 'application/x-www-form-urlencoded', 
                    'Authorization' : 'Basic ' + btoa(this.clientId + ':' + this.clientSecret)
                },
                body: 'grant_type=client_credentials'
            });
            const data = await result.json()
            this.token = data.access_token
            this.startupDone = true
        },
        /*getTokenYEP(){
            return 'https://accounts.spotify.com/api/token', {
                method: 'POST',
                headers: {
                    'client_id' : encodeURIComponent(this.clientId),
                    '&client_secret' : encodeURIComponent(this.clientSecret),
                    '&grant_type' : 'authorization_code',
                    '&code' : encodeURIComponent(this.goodtoken),
                    '&redirect_uri' : encodeURIComponent(this.redirectURI)
                },
                body: 'grant_type=client_credentials'
            }
        },*/
        getPermToken(){
            return 'https://accounts.spotify.com/authorize?client_id=' + this.clientId
            + '&redirect_uri=' + encodeURIComponent(this.redirectURI)
            + '&scope=' + encodeURIComponent(this.scopes)
            + '&response_type=code';
        },
        async getGenres(){
            const result = await fetch(`https://api.spotify.com/v1/browse/categories`, {
                method: 'GET',
                headers: { 'Authorization' : 'Bearer ' + this.token}
            });
            const data = await result.json()
            this.genres = data.categories.items
            this.getCategoriesDone = true
        },
        async getDeviceId(){
            const result = await fetch(`https://api.spotify.com/v1/me/player/devices`, {
                method: 'GET',
                headers: { 'Authorization' : 'Bearer ' + this.token}
            })
            const data = await result.json()
            this.deviceId = data.devices.id
            this.deviceIdGotten = true
        },
        getFragmentIdentifier(){
            const urlParams = new URLSearchParams(window.location.search);
            this.goodtoken = urlParams.get('code');
        }
    }
}).mount("#app")