const baseUri = ""
Vue.createApp({
    data() {
        return {
            clientId: '849bcded0aa04ffa855b5bd3381c7284',
            clientSecret: '25bed5e8f08a43ac9f914b920dae2b4b',
            token: "",
            startupDone: false,
            getCategoriesDone: false,
            genres: [],
            deviceId: "",
            deviceIdGotten: false
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
        }
    }
}).mount("#app")