import api from "../services/api";
import { useState } from "react";
import { AxiosError } from "axios";

interface IFetchState<T> {
    data: T | null;
    loading: boolean;
    error: string | null;
}
type  Method = "GET" | "POST" | "PUT" | "DELETE" | "PATCH"; 
function useRequest<T> (url : string,method : Method, body?: any) : IFetchState<T> {
    const [data, setData] = useState<T | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    
    setLoading(true);
    setError(null);
    try{
         api({
            method: method,
            url: url,
            data: body
            
        }).then((res) => {
            setData(res.data);
        });
        setError(null);
        
    }
    catch (err) {
        const axiosError = err as AxiosError;
        setError(axiosError.message);
        return {data: null, loading: false, error: axiosError.message};
    }
    finally {
        setLoading(false);
    }

    return {data, loading, error};
}
export default useRequest;