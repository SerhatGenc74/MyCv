import {useEffect, useState} from "react";
import { AxiosError } from 'axios';
import api from "../services/api";

interface IFetchState<T>{
    data: T | null;
    loading: boolean;
    error: string | null;
}
 function useFetch<T>(url : string) : IFetchState<T> {

    const [data, setData] = useState<T | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);


     useEffect(() => {
        const fetchData = async () => 
        {
            setLoading(true);
            try{
                const response = await api.get<T>(url);
                setData(response.data);
                setError(null);
            }
            catch (err) {
                const axiosError = err as AxiosError;
                setError(axiosError.message);
                setData(null);
            }
            finally {
                setLoading(false);
            }
        }
        fetchData();
     },[url])

     return {data, loading, error};
}
export default useFetch;