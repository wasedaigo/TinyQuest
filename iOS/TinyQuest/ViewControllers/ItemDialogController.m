//
//  ItemDialogController.m
//  TinyQuest
//
//  Created by Daigo Sato on 11/27/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import "ItemDialogController.h"

@implementation ItemDialogController
@synthesize view, delegate;
- (id)init {
    self = [super init];
    if (self != nil)
        [self loadNib];
    
    return self;
}

- (BOOL)loadNib 
{
    [[NSBundle mainBundle] loadNibNamed:@"ItemDetailView" owner:self options:nil];
    

    return YES;
}

- (void)slideIn
{
    self.view.userInteractionEnabled = NO;
    [UIView beginAnimations:nil context:nil];
    [UIView setAnimationDelegate:self];
    [UIView setAnimationDidStopSelector:@selector(onSlideFinished:finished:context:)];
    [UIView setAnimationDuration:0.5];
    [UIView setAnimationCurve:UIViewAnimationCurveEaseOut];
    self.view.frame = CGRectMake(0, self.view.frame.origin.y, self.view.frame.size.width, self.view.frame.size.height);
    [UIView commitAnimations];
}

- (void)slideOut
{
    self.view.userInteractionEnabled = NO;
    [UIView beginAnimations:nil context:nil];
    [UIView setAnimationDelegate:self];
    [UIView setAnimationDidStopSelector:@selector(onSlideFinished:finished:context:)];
    [UIView setAnimationDuration:0.5];
    [UIView setAnimationCurve:UIViewAnimationCurveEaseOut];
    self.view.frame = CGRectMake(320, self.view.frame.origin.y, self.view.frame.size.width, self.view.frame.size.height);
    [UIView commitAnimations];
}

- (IBAction)backButtonClicked:(id)sender
{
    [self slideOut];
    [delegate slideIn];
}

-(void)onSlideFinished:(NSString *)animationID finished:(NSNumber *)finished context:(void *)context
{
    self.view.userInteractionEnabled = YES;
}
@end
