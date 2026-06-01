import { HubConnectionBuilder } from "@microsoft/signalr";

// const connection = new HubConnectionBuilder()
//   .withUrl("https://localhost:7280/hubs/dashboard")
//   .withAutomaticReconnect()
//   .build();

// connection.on("dashboard:update", (data) => {
//   console.log("LIVE DASHBOARD:", data);

//   setStats(data); // твій state
// });

// connection.start();