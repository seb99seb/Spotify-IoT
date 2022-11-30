import express from "express";

const app = express();

app.set("views","./views");
app.set("view engine","pug");

app.use(express.static("public"));

app.get("/",function(req,res) => {
    res.render("index");
});

app.get("/authorize"), (req,res)=>{
    console.log(
        "Your app is listening on http://localhost" + AudioListener.address().port
    );
}