import React, { useEffect, useState} from 'react';
import axios from "axios"

const App = () => {
	const baseURL = "https://azureprojecttime.azurewebsites.net/api/Get_Time";
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState([])

  useEffect(() => {
    const fetchData = async () =>{
      setLoading(true);
      try {
        const {data: response} = await axios.get('https://azureprojecttime.azurewebsites.net/api/Get_Time');
        setData(response);
      } catch (error) {
        console.error(error.message);
      }
      setLoading(false);
    }
    fetchData();
  }, []);

  return (
    <div>
      {loading && <div>Loading</div>}
      {!loading && (
        <div>
          <h2>czasy</h2>
          {data.map(item => (<div>{item.czas}</div>))}
        </div>
      )}
    </div>
  )
};
export default App;
