import { useState,useEffect } from 'react'

import axios from 'axios';
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

function App() {
  const [name, setName] = useState("")
  const [todos, setTodos] = useState([])
  const [ isUpdated,setIsUpdated] = useState(false)
  function DergoTodoBackEnd(e) {
    e.preventDefault()
    const todoItem = {
      Name :name,
      IsComplete :false
    }
    axios.post( `https://localhost:7168/api/todoitems`,{ name,IsComplete :true })
    .then(res => {
      console.log(res);
      console.log(res.data);
      setIsUpdated(!isUpdated)
    })
    .catch(error=> console.log(error)
    )
    
  }
  useEffect(() => {
    // Update the document title using the browser API
    axios.get( `https://localhost:7168/api/todoitems`)
    .then(res => {
      setTodos(res.data)
      console.log(res.data)
    })
    .catch(error=> console.log(error)
    )
    
  },[isUpdated]);

  function UpdateCheck(todo) {
    todo.isComplete = !todo.isComplete
   const Name = todo.name
   const isComplete= todo.isComplete
   const id= todo.id
   console.log(name,isComplete)
    axios.post( `https://localhost:7168/api/todoitems/update/${todo.id}`,{ id,Name,isComplete })
    .then(res => {
      console.log(res);
      console.log(res.data);
      setIsUpdated(!isUpdated)
    })
    .catch(error=> console.log(error)
    )
    
  }
  return (
    <>
     <form onSubmit={DergoTodoBackEnd}>
      <h2>Add Item</h2>
      <div>
        <label htmlFor="">Name </label>
        <input type="text" placeholder='Add Name' onChange={(e) => setName(e.target.value)} />
      </div>
      <button type='submit'>Shto Todo</button>
     </form>
     {todos.map((todo,index)=>{
      return <p key={todo.id}>{todo.name}  <input type='checkbox' checked={todo.isComplete} onChange={()=>UpdateCheck(todo)}/></p>
     })}
     <p>{JSON.stringify(isUpdated)}</p>
    </>
  )
}

export default App
