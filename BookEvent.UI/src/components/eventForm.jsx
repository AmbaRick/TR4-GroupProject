import React, { useState } from "react";
import axios from "axios"

const EventForm = () => {

  const eventBookingApi = "https://localhost:7150/api/Event";

  let events = [
    { id: "id1", name: "Epic Moments Productions: Crafting Unforgettable Experiences" },
    { id: "id2", name: "Dreamscape Events: Where Fantasies Come to Life" },
    { id: "id3", name: "Enchanted Affairs: Turning Dreams into Reality" },
    { id: "id4", name: "Whimsical Wonder Events: A World of Magic and Charm" },
    { id: "id5", name: "Spectacular Soirees: Creating Memories That Last" },
  ]
  const [eventBooking, setEventBooking] = useState({
    eventId: "",
    eventName: "",
    seatsBooked: "1",
    emailAddress: "",
  })

  //let [selectedEvent, setEvent] = useState("");

  let handleChange = (e) => {
    e.preventDefault();
    const { name, value } = e.target;
    setEventBooking({ ...eventBooking, [name]: value });
  }

  let handleSubmit = async (e) => {
    e.preventDefault();

    var selectedEvent = events.find(item => item.id === eventBooking.eventId);
    eventBooking.eventName = selectedEvent.name;
    console.log("Event Booking: ", eventBooking)

   
      axios
        .post(eventBookingApi, {
          eventId: eventBooking.eventId,
          eventName: eventBooking.eventName,
          emailAddress: eventBooking.emailAddress,
          seatsBooked: eventBooking.seatsBooked
        })
        .then((response) => {
            console.log('Form submitted successfully!',response.status, response.data);
            setEventBooking({
              eventId: "",
              eventName: "",
              seatsBooked: "",
              emailAddress: "",
            });         
        })
        .catch((error)=> {console.log("Error occured..",error)});   
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="max-w-md mx-auto p-4 bg-white rounded-md shadow-md"
    >
      <div className="mb-4">
        <label className="block text-gray-700">Event</label>
        <select
          name="eventId"
          onChange={handleChange}
          className="w-full mt-1 p-2 border border-gray-300 rounded-md"
        >
          <option value="Select an Event"> -- Select an Event -- </option>
          {events.map((event) => <option key={event.id} value={event.id}>{event.name}</option>)}
        </select>
      </div>
      <div className="mb-4">
        <label className="block text-gray-700">Number of Tickets</label>
        <select
          name="seatsBooked"
          onChange={handleChange}
          className="w-full mt-1 p-2 border border-gray-300 rounded-md"
        >
          {
            [...Array(10)].map((_, i) => i + 1)
              .map(i => <option key={i} value={i}>{i}</option>)
          }
        </select>
      </div>
      <div className="mb-4">
        <label className="block text-gray-700">Email</label>
        <input
          type="email"
          name="emailAddress"
          value={eventBooking.emailAddress}
          onChange={handleChange}
          className="w-full mt-1 p-2 border border-gray-300 rounded-md"
        />
      </div>
      <button
        type="submit"
        className="w-full bg-blue-500 text-white p-2 rounded-md hover:bg-blue-600"
      >
        Submit
      </button>
    </form>
  );
};

export default EventForm;
