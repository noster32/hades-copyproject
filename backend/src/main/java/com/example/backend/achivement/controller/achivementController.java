package com.example.backend.achivement.controller;

import com.example.backend.achivement.service.achivementService;
import com.example.backend.model.AchivementModel;
import com.example.backend.model.UserModel;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

@Controller
@ResponseBody
@RequestMapping("/api/achivement/*")
public class achivementController {

    @Autowired
    private achivementService achivementService;

    @RequestMapping("/getachivement")
    public AchivementModel getAchivement(@RequestBody
                                             AchivementModel achivementModel){
        return achivementService.getAchivement(achivementModel);
    }
}
