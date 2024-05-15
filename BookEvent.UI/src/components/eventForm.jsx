import React, { useState } from "react";
import { ToastContainer, toast } from 'react-toastify';
import axios from "axios"

const EventForm = () => {

  const eventBookingApi = import.meta.env.VITE_API_URL;

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

  let onSubmit = async (e) => {
    e.preventDefault();

    var selectedEvent = events.find(item => item.id === eventBooking.eventId);
    
    eventBooking.eventName = selectedEvent.name;
    console.log("Event Booking: ", eventBooking)
    axios
      .post(eventBookingApi, eventBooking)
      .then((response) => {
        console.log('Form submitted successfully!', response.status, response.data);
        toast.success("Your order is in progress. You will receive an email shorty!", {autoClose: 10000});

        setEventBooking({
          eventId: "",
          eventName: "",
          seatsBooked: "1",
          emailAddress: "",
        });
      })
      .catch((error) => { 
        console.log("Error occured..", error); 
        toast.error("Something went wrong. Please try again later.", {autoClose: 10000});
      });
  };

  return (
    <form
      onSubmit={onSubmit}
      className="max-w-md mx-auto p-4 bg-white rounded-md shadow-md"
    >
      <div className="mb-4">
        <label className="block text-gray-700">Events</label>
        <select
          name="eventId"
          onChange={handleChange}
          value={eventBooking.eventId}
          className="w-full mt-1 p-2 border border-gray-300 rounded-md"
          required
        >
          <option value=""> -- Select an Event -- </option>
          {events.map((event) =>
            <option
              key={event.id}
              value={event.id}
            >
              {event.name}
            </option>
          )}
        </select>
      </div>
      <div className="mb-4">
        <label className="block text-gray-700">Number of Tickets</label>
        <select
          name="seatsBooked"
          onChange={handleChange}
          value={eventBooking.seatsBooked}
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
          required
        />
      </div>
      <button
        type="submit"
        className="w-full bg-blue-500 text-white p-2 rounded-md hover:bg-blue-600"
      >
        Buy
      </button>
    </form>
  );
};

export default EventForm;
