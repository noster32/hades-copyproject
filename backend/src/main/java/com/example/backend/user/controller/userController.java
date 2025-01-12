package com.example.backend.user.controller;

import com.example.backend.model.UserModel;
import com.example.backend.user.service.userService;
import lombok.extern.log4j.Log4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.ArrayList;

@Controller
@ResponseBody
@RequestMapping("/api/user/*")
public class userController {

    @Autowired
    private userService uService;

    @RequestMapping("/getuser")
    public UserModel getUser(@RequestBody
                                         UserModel userModel){

        System.out.println(uService.getUser(userModel));
        return uService.getUser(userModel);
    }
}
