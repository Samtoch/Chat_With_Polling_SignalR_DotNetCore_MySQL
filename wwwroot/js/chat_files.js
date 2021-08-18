const toBase64 = (file) =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });

const createNewImg = (filedata = {}) => {
  return `
  <figure class="alert alert-primary">
    <figcaption>${filedata.user}</figcaption>
    <img src='${filedata.data}' alt="photo-${filedata.user}" class='img-receive' />
  </figure>
  `;
};

let conectChat = new signalR.HubConnectionBuilder()
  .withUrl("/fileshub")
  .configureLogging(signalR.LogLevel.Information)
  .build();

document.getElementById("SendMessageBtn").disabled = true;

conectChat
  .start()
  .then(function () {
    document.getElementById("SendMessageBtn").disabled = false;
    console.log("[signalR Conected]");
  })
  .catch(function (err) {
    return console.error(err.toString());
  });

conectChat.on("ReceiveImageMessage", function (file) {
  let li = document.createElement("li");
  li.innerHTML = createNewImg(file);
  document.getElementById("messagesList").appendChild(li);
});

document
  .getElementById("SendMessageBtn")
  .addEventListener("click", async function (event) {
    event.preventDefault();
    let fileList = document.getElementById("filesSending").files;
    let image = {
      data: null,
      header: null,
      name: null,
      user: null,
    };

    if (fileList && fileList.length) {
      image.header = "data: " + fileList[0].type + ";base64, ";
      image.name = fileList[0].name;
      image.user = document.getElementById("userInput").value;
      image.data = await toBase64(fileList[0]);

      conectChat.invoke("SendImageMessage", image).catch(function (err) {
        console.error({ err });
      });
    }
  });
