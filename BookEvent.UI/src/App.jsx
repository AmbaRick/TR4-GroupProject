 import './App.css'
import EventForm from './components/eventForm.jsx'
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


function App() {
  return (
    <div className="min-h-height  items-center justify-center bg-gray-100">
      <h4 className="mb-8 text-4xl font-bold leading-none tracking-tight text-center  md:text-5xl lg:text-6xl">Event Booking</h4>
      <ToastContainer />
      <EventForm />
    </div>
  );
};


export default App
