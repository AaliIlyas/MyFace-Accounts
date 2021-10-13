import React from 'react';
import { Page } from "../Page/Page";
import { fetchPosts } from "../../Api/apiClient";
import { PostCard } from "../../Components/PostCard/PostCard";
import { InfiniteList } from "../../Components/InfititeList/InfiniteList";
import { Link } from "react-router-dom";
import './Feed.scss';

export function Feed(): JSX.Element {
    async function validateButton() {
        await fetch("https://localhost:5001/login/validate", {
            method: 'GET',
            credentials: 'include',
        })
        .then(r => r.json())
        .then(r => console.log(r));
    }

    return (
        <Page containerClassName={"feed"}>
            <h1 className="title">Feed</h1>
            <button onClick={validateButton}>Validate</button>
                <InfiniteList fetchItems={fetchPosts} renderItem={post => <PostCard key={post.id} post={post} />} />
                <Link className="create-post" to="/new-post">+</Link>
        </Page>
    );
}