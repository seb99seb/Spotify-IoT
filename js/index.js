const baseURI='https://api.spotify.com/v1'

const app = Vue.createApp({
    data() {
        return {
            word: "ok",
            words: [],
            Message: ""
        }
    },
    computed:{
        wordDisplay(){
            return `${this.words+" "}`
        }
    },
    methods: {
        // add here
        Save(){
            this.words.push(this.word)
        },
        Show(){
            //ok
        },
        Clear(){
            this.words=[]
        }
    }
})