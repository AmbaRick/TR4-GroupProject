 import './App.css'
import EventForm from './components/eventForm.jsx'



function App() {
  return (
    <div className="min-h-height  items-center justify-center bg-gray-100">
      <h4 className="mb-8 text-4xl font-bold leading-none tracking-tight text-center  md:text-5xl lg:text-6xl">Event Booking</h4>
      <EventForm />
    </div>
  );
};


export default App
